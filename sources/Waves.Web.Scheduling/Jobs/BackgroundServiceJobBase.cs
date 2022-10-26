using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Waves.Web.Scheduling.Jobs;

/// <summary>
/// Background service base.
/// </summary>
public abstract class BackgroundServiceJobBase : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates new instance of <see cref="BackgroundServiceJobBase"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="serviceProvider">Service provider.</param>
    protected BackgroundServiceJobBase(
        ILogger<BackgroundServiceJobBase> logger,
        IServiceProvider serviceProvider)
    {
        Logger = logger;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger<BackgroundServiceJobBase> Logger { get; }

    /// <inheritdoc />
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation(
            $"Job {nameof(BackgroundServiceJobBase)} is stopping.");

        await base.StopAsync(stoppingToken);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation(
            $"Job {nameof(BackgroundServiceJobBase)} is starting.");

        using var scope = _serviceProvider.CreateScope();
        await ExecuteJobAsync(scope, stoppingToken);
    }

    /// <summary>
    /// Executes job with created <see cref="IServiceScope"/>.
    /// </summary>
    /// <param name="scope">Service scope.</param>
    /// <param name="stoppingToken">Stopping token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task ExecuteJobAsync(IServiceScope scope, CancellationToken stoppingToken);
}
