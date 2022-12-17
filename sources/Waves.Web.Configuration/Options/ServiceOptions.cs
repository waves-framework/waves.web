using System.Collections;
using System.Collections.Generic;

namespace Waves.Web.Configuration.Options;

/// <summary>
/// Service options.
/// </summary>
public class ServiceOptions
{
    /// <summary>
    /// Gets or sets logging.
    /// </summary>
    public LoggingOption Logging { get; set; }

    /// <summary>
    /// Gets or sets authentication options.
    /// </summary>
    public AuthenticationOptions Authentication { get; set; } = new AuthenticationOptions();

    /// <summary>
    /// Gets or sets credentials.
    /// </summary>
    public Dictionary<string, CredentialOption> Credentials { get; set; }

    /// <summary>
    /// Gets or sets authentication secrets.
    /// </summary>
    public Dictionary<string, SecretOption> Secrets { get; set; }

    /// <summary>
    /// Gets or sets urls.
    /// </summary>
    public Dictionary<string, string> Urls { get; set; }

    /// <summary>
    /// Gets or sets urls.
    /// </summary>
    public Dictionary<string, string> Settings { get; set; }
}
