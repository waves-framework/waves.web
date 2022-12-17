using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Web.Configuration.Services.Interfaces;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Security;
using Waves.Web.Identity.Tools;
using Waves.Web.Scheduling.Jobs;

namespace Waves.Web.Identity.Jobs;

/// <summary>
/// Initializes admin user job.
/// </summary>
/// <typeparam name="TContext">Context type.</typeparam>
/// <typeparam name="TUser">Type of user.</typeparam>
/// <typeparam name="TRole">Type of role.</typeparam>
public class InitializeAdminUserJob<TContext, TUser, TRole> : BackgroundServiceJobBase
    where TContext : WavesIdentityDatabaseContext<TContext, TUser, TRole>
    where TUser : UserEntity
    where TRole : UserRoleEntity
{
    private readonly IConfigurationService _configurationService;

    /// <summary>
    /// Creates new instance of <see cref="InitializeAdminUserJob{T}"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="configurationService">Configuration.</param>
    public InitializeAdminUserJob(
        ILogger<InitializeAdminUserJob<TContext, TUser, TRole>> logger,
        IServiceProvider serviceProvider,
        IConfigurationService configurationService)
        : base(logger, serviceProvider)
    {
        _configurationService = configurationService;
    }

    /// <summary>
    /// Executes job.
    /// </summary>
    /// <param name="scope">Scope.</param>
    /// <param name="stoppingToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task ExecuteJobAsync(IServiceScope scope, CancellationToken stoppingToken)
    {
        try
        {
            await DatabaseInitializationTools.WaitForInitialization(this, scope, Logger, cancellationToken: stoppingToken);

            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var users = context.Users;

            var adminCredentials = _configurationService.GetCredentialEntity("Admin");
            if (adminCredentials != null)
            {
                var admin = new UserEntity(
                    adminCredentials.Username,
                    adminCredentials.Username + "@waves-framework.io",
                    adminCredentials.Password.ToSha256(),
                    Roles.AdministratorRole);

                if (users.FirstOrDefault(x => x.UserName.Equals(admin.UserName)) == null)
                {
                    users.Add(admin);
                    await context.SaveChangesAsync(stoppingToken);
                    Logger.LogInformation("Created default admin user - {Name}", admin.UserName);
                }
                else
                {
                    Logger.LogWarning("Administrator user ({Name}) already exists", admin.UserName);
                }
            }
            else
            {
                Logger.LogWarning("Default administrator user credentials not found");
            }
        }
        catch (Exception e)
        {
            Logger.LogError("Error occured while creating default administrator user: {Message}", e.Message);
        }
    }
}
