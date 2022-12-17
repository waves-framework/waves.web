using System.Threading.Tasks;
using Waves.Web.Configuration.Options;

namespace Waves.Web.Configuration.Services.Interfaces;

/// <summary>
/// Interface for configuration service.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Gets authentication options.
    /// </summary>
    /// <returns>Returns options.</returns>
    AuthenticationOptions GetAuthentication();

    /// <summary>
    /// Gets login / password.
    /// </summary>
    /// <param name="name">Service name.</param>
    /// <returns>Returns token.</returns>
    CredentialOption GetCredential(string name);

    /// <summary>
    /// Gets token.
    /// </summary>
    /// <param name="name">Service name.</param>
    /// <returns>Returns token.</returns>
    SecretOption GetSecret(string name);

    /// <summary>
    /// Gets connection string.
    /// </summary>
    /// <param name="name">Service name.</param>
    /// <returns>Returns connection string.</returns>
    string GetConnectionUrl(string name);

    /// <summary>
    /// Gets setting.
    /// </summary>
    /// <param name="name">Setting name.</param>
    /// <returns>Returns setting value.</returns>
    string GetSetting(string name);

    /// <summary>
    /// Saves configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Save();
}
