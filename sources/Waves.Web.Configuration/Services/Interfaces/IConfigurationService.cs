using System.Threading.Tasks;
using Waves.Web.Configuration.Entities;

namespace Waves.Web.Configuration.Services.Interfaces;

/// <summary>
/// Interface for configuration service.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Gets login / password.
    /// </summary>
    /// <param name="name">Service name.</param>
    /// <returns>Returns token.</returns>
    Credential GetCredentialEntity(string name);

    /// <summary>
    /// Gets token.
    /// </summary>
    /// <param name="name">Service name.</param>
    /// <returns>Returns token.</returns>
    Authentication GetAuthenticationEntity(string name);

    /// <summary>
    /// Gets connection string.
    /// </summary>
    /// <param name="name">Service name.</param>
    /// <returns>Returns connection string.</returns>
    string GetUrl(string name);

    /// <summary>
    /// Gets token secret.
    /// </summary>
    /// <returns>Returns token secret.</returns>
    string GetTokenSecret();

    /// <summary>
    /// Saves configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Save();
}
