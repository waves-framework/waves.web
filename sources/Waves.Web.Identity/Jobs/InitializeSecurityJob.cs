using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Security;
using Waves.Web.Identity.Tools;
using Waves.Web.Scheduling.Jobs;

namespace Waves.Web.Identity.Jobs;

/// <summary>
/// Initialize security job.
/// </summary>
public class InitializeSecurityJob : BackgroundServiceJobBase
{
    /// <summary>
    /// Creates new instance of <see cref="InitializeSecurityJob"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="serviceProvider">Service provider.</param>
    public InitializeSecurityJob(
        ILogger<InitializeSecurityJob> logger,
        IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
    }

    /// <inheritdoc />
    protected override async Task ExecuteJobAsync(IServiceScope scope, CancellationToken stoppingToken)
    {
        try
        {
            await DatabaseInitializationTools.WaitForInitialization(this, scope, Logger, cancellationToken: stoppingToken);

            Logger.LogInformation("Role update started");

            var manager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRoleEntity>>();

            foreach (var role in Roles.Get())
            {
                try
                {
                    var result = await manager.CreateAsync(new UserRoleEntity(role));
                    if (result.Succeeded)
                    {
                        continue;
                    }

                    if (result.Errors.Count() != 1)
                    {
                        throw new Exception($"The {role} wasn't added to database");
                    }

                    var error = result.Errors.First();

                    if (error.Code == "DuplicateRoleName")
                    {
                        continue;
                    }

                    throw new Exception($"Unknown error");
                }
                catch (Exception e)
                {
                    Logger.LogInformation("Role {Role} update failed: {Message}", role, e.Message);
                }
            }

            Logger.LogInformation("Roles update completed");
        }
        catch (Exception e)
        {
            Logger.LogInformation("Roles update failed: {Message}", e.Message);
        }
    }
}
