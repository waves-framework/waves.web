using System.Collections;
using System.Collections.Generic;
using Waves.Web.Configuration.Entities;

namespace Waves.Web.Configuration.Options;

/// <summary>
/// Service options.
/// </summary>
public class ServiceOptions
{
    /// <summary>
    /// Gets or sets logging.
    /// </summary>
    public Logging Logging { get; set; }

    /// <summary>
    /// Gets or sets credentials.
    /// </summary>
    public Dictionary<string, Credential> Credentials { get; set; }

    /// <summary>
    /// Gets or sets authentication secrets.
    /// </summary>
    public Dictionary<string, Authentication> Authentication { get; set; }

    /// <summary>
    /// Gets or sets urls.
    /// </summary>
    public Dictionary<string, string> Urls { get; set; }

    /// <summary>
    /// Gets or sets urls.
    /// </summary>
    public Dictionary<string, string> Settings { get; set; }

    /// <summary>
    /// Gets or sets token secret.
    /// </summary>
    public string TokenSecret { get; set; }
}
