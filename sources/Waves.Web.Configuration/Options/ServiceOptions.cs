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
    public LoggingOptions Logging { get; set; }

    /// <summary>
    /// Gets or sets credentials.
    /// </summary>
    public ICollection<CredentialEntity> Credentials { get; set; }
}
