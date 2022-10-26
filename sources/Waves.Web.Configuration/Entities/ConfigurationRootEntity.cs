using System.Collections.Generic;

namespace Waves.Web.Configuration.Entities;

/// <summary>
/// Configuration entity.
/// </summary>
public class ConfigurationRootEntity
{
    /// <summary>
    /// Gets or sets logging.
    /// </summary>
    public LoggingEntity Logging { get; set; }

    /// <summary>
    /// Gets or sets connection string dictionary.
    /// </summary>
    public Dictionary<string, string> Urls { get; set; } = new ();

    /// <summary>
    /// Gets or sets tokens dictionary.
    /// </summary>
    public Dictionary<string, AuthenticationEntity> Tokens { get; set; } = new ();

    /// <summary>
    /// Gets or sets credentials entities.
    /// </summary>
    public Dictionary<string, CredentialEntity> Credentials { get; set; } = new ();

    /// <summary>
    /// Gets or sets token secret.
    /// </summary>
    public string TokenSecret { get; set; }
}
