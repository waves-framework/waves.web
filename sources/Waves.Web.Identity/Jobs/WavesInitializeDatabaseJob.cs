using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Web.Identity.Services.Interfaces;
using Waves.Web.Scheduling.Jobs;

namespace Waves.Web.Identity.Jobs;

/// <summary>
/// Initialize database job.
/// </summary>
public class WavesInitializeDatabaseJob : BackgroundServiceJobBase
{
    /// <summary>
    /// Creates new instance of <see cref="WavesInitializeDatabaseJob"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="serviceProvider">Service provider.</param>
    public WavesInitializeDatabaseJob(
        ILogger<WavesInitializeDatabaseJob> logger,
        IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
    }

    /// <inheritdoc />
    protected override async Task ExecuteJobAsync(IServiceScope scope, CancellationToken stoppingToken)
    {
        var services = scope.ServiceProvider;
        try
        {
            Logger.LogInformation("Database initialization started");
            var initializationService = services.GetService<IWavesIdentityDatabaseContextInitializationService>();
            if (initializationService != null)
            {
                await initializationService.InitializeDatabase();

                if (initializationService.IsInitialized)
                {
                    Logger.LogInformation("Database initialization completed");
                }
                else
                {
                    throw new Exception("Unknown error");
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogCritical("Database initialization failed", e.Message);
        }
    }
}
