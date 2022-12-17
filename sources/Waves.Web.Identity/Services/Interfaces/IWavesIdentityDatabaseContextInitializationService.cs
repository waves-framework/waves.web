using System.Threading.Tasks;

namespace Waves.Web.Identity.Services.Interfaces;

/// <summary>
/// Database initialize service.
/// </summary>
public interface IWavesIdentityDatabaseContextInitializationService
{
    /// <summary>
    /// Gets whether database is initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initializes database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task InitializeDatabase();
}
