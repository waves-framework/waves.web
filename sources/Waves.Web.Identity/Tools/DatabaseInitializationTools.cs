using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Web.Identity.Services.Interfaces;
using Waves.Web.Scheduling.Jobs;

namespace Waves.Web.Identity.Tools;

/// <summary>
/// Database initialization waiter methods.
/// </summary>
public static class DatabaseInitializationTools
{
    /// <summary>
    /// Waits for initialization.
    /// </summary>
    /// <param name="job">Job.</param>
    /// <param name="scope">Service scope.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="delay">Wait delay.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task WaitForInitialization(BackgroundServiceJobBase job, IServiceScope scope, ILogger logger, int delay = 10, CancellationToken cancellationToken = default)
    {
        if (job == null)
        {
            throw new ArgumentNullException(nameof(job));
        }

        var delaySpan = TimeSpan.FromSeconds(delay);
        var contextInitializationService = scope.ServiceProvider.GetService<IWavesIdentityDatabaseContextInitializationService>();
        do
        {
            logger.LogWarning("Background service {Job} waits until the database initialization is completed", nameof(job));
            await Task.Delay(delaySpan, cancellationToken);
        }
        while (contextInitializationService is { IsInitialized: false });
    }
}
