using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Waves.Web.Configuration.Services;
using Waves.Web.Configuration.Services.Interfaces;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Jobs;
using Waves.Web.Identity.Security.Options;
using Waves.Web.Identity.Services;
using Waves.Web.Identity.Services.Interfaces;
using Waves.Web.Identity.Tools;
using Waves.Web.Identity.Utils;

namespace Waves.Web.Identity.Extensions;

/// <summary>
///     Extensions to the Startup class for initializing shared services between different servers.
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    ///     Adds common services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <typeparam name="T">Type of database context.</typeparam>
    /// <returns>Returns service collection.</returns>
    public static IServiceCollection AddIdentityServices<T>(
        this IServiceCollection services,
        IConfiguration configuration)
        where T : WavesIdentityDatabaseContext<T>
    {
        // scopes
        services.AddScoped<IUserService, UserService<T>>();
        services.AddScoped<ITokenService, TokenService>();

        // singletons
        services.AddSingleton<IDatabaseContextInitializationService, DatabaseContextInitializationService<T>>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();

        // background jobs
        services.AddHostedService<InitializeSecurityJob>();
        services.AddHostedService<InitializeDatabaseJob>();
        services.AddHostedService<InitializeAdminUserJob<T>>();

        return services;
    }

    /// <summary>
    ///     Initializes database.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <typeparam name="T">Type of database context.</typeparam>
    public static void InitializeDatabase<T>(this IServiceCollection services, IConfiguration configuration)
        where T : WavesIdentityDatabaseContext<T>
    {
        services.AddDefaultIdentity<UserDbEntity>()
            .AddRoles<UserRoleDbEntity>()
            .AddUserManager<UserManager<UserDbEntity>>()
            .AddRoleManager<RoleManager<UserRoleDbEntity>>()
            .AddEntityFrameworkStores<T>();

        services.AddEntityFrameworkNpgsql()
            .AddDbContextPool<T>(options =>
            {
                var userName = configuration
                    .GetSection("Credentials")
                    .GetSection("Psql")
                    .GetSection("Username")
                    .Value;

                var password = configuration
                    .GetSection("Credentials")
                    .GetSection("Psql")
                    .GetSection("Password")
                    .Value;

                var url = configuration
                    .GetSection("Urls")
                    .GetSection("Psql")
                    .Value;

                var (ip, port) = SplitStringUtils.GetIpAndPort(url);
                var connectionString =
                    $"Server={ip};Database={Constants.DatabaseNameKey};Port={port};User Id={userName};Password={password};";

                options.UseNpgsql(
                    connectionString,
                    optionsBuilder => { });
            });
    }

    /// <summary>
    /// Initializes authentication.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <param name="configuration">Configuration.</param>
    public static void InitializeAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var securityKey = configuration.GetValue(Security.Constants.TokenSecretKey, "0000");

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = AuthenticationTools.Authenticate,
                    OnAuthenticationFailed = _ => Task.CompletedTask
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthenticationOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = AuthenticationOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthenticationOptions.GetSymmetricSecurityKey(securityKey),
                };
            });
    }
}
