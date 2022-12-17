using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Waves.Web.Configuration.Options;
using Waves.Web.Configuration.Services;
using Waves.Web.Configuration.Services.Interfaces;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Jobs;
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
    /// <typeparam name="TContext">Context type.</typeparam>
    /// <typeparam name="TUser">Type of user.</typeparam>
    /// <typeparam name="TRole">Type of role.</typeparam>
    /// <returns>Returns service collection.</returns>
    public static IServiceCollection AddIdentityServices<TContext, TUser, TRole>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TContext : WavesIdentityDatabaseContext<TContext, TUser, TRole>
        where TUser : WavesUserEntity
        where TRole : WavesUserRoleEntity
    {
        // scopes
        services.AddScoped<IWavesUserService, WavesUserService<TContext, TUser, TRole>>();
        services.AddScoped<IWavesTokenService, WavesTokenService>();

        // singletons
        services.AddSingleton<IWavesIdentityDatabaseContextInitializationService, WavesIdentityDatabaseContextInitializationService<TContext, TUser, TRole>>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();

        // background jobs
        services.AddHostedService<WavesInitializeSecurityJob>();
        services.AddHostedService<WavesInitializeDatabaseJob>();
        services.AddHostedService<WavesInitializeAdminUserJob<TContext, TUser, TRole>>();

        return services;
    }

    /// <summary>
    ///     Initializes database.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <typeparam name="TContext">Context type.</typeparam>
    /// <typeparam name="TUser">Type of user.</typeparam>
    /// <typeparam name="TRole">Type of role.</typeparam>
    public static void InitializeDatabase<TContext, TUser, TRole>(this IServiceCollection services, IConfiguration configuration)
        where TContext : WavesIdentityDatabaseContext<TContext, TUser, TRole>
        where TUser : WavesUserEntity
        where TRole : WavesUserRoleEntity
    {
        services.AddDefaultIdentity<WavesUserEntity>()
            .AddRoles<WavesUserRoleEntity>()
            .AddUserManager<UserManager<WavesUserEntity>>()
            .AddRoleManager<RoleManager<WavesUserRoleEntity>>()
            .AddEntityFrameworkStores<TContext>();

        services.AddEntityFrameworkNpgsql()
            .AddDbContextPool<TContext>(options =>
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
                    _ => { });
            });
    }

    /// <summary>
    /// Initializes authentication.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <param name="configuration">Configuration.</param>
    public static void InitializeAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.Get<ServiceOptions>();

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
                    ValidIssuer = options.Authentication.Issuer,
                    ValidateAudience = true,
                    ValidAudience = options.Authentication.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthenticationOptions.GetSymmetricSecurityKey(options.Authentication.TokenSecret),
                };
            });
    }
}
