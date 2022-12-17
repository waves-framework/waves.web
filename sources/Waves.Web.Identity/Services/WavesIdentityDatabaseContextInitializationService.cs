using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Services.Interfaces;

namespace Waves.Web.Identity.Services;

/// <summary>
/// Database initialization service.
/// </summary>
/// <typeparam name="TContext">Context type.</typeparam>
/// <typeparam name="TUser">Type of user.</typeparam>
/// <typeparam name="TRole">Type of role.</typeparam>
public class WavesIdentityDatabaseContextInitializationService<TContext, TUser, TRole>
    : IWavesIdentityDatabaseContextInitializationService
    where TContext : WavesIdentityDatabaseContext<TContext, TUser, TRole>
    where TUser : WavesUserEntity
    where TRole : WavesUserRoleEntity
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WavesIdentityDatabaseContextInitializationService<TContext, TUser, TRole>> _logger;

    /// <summary>
    /// Database initialization service.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="logger">Logger.</param>
    public WavesIdentityDatabaseContextInitializationService(
        IServiceProvider serviceProvider,
        ILogger<WavesIdentityDatabaseContextInitializationService<TContext, TUser, TRole>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public bool IsInitialized { get; private set; }

    /// <inheritdoc />
    public async Task InitializeDatabase()
    {
        _logger.LogInformation("Applying migrations...");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var migrations = (await context.Database.GetPendingMigrationsAsync()).ToList();

            if (migrations.Any())
            {
                await context.Database.MigrateAsync();

                foreach (var migration in migrations)
                {
                    _logger.LogInformation("Database migration {Migration} applied", migration);
                }
            }

            IsInitialized = true;
        }
        catch (Exception e)
        {
            _logger.LogCritical("Database initialization failed: {Message}", e.Message);
        }
    }
}
