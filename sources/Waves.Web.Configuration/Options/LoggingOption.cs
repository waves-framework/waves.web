using System.Collections.Generic;

namespace Waves.Web.Configuration.Options;

/// <summary>
/// Logging configuration entity.
/// </summary>
public class LoggingOption
{
    /// <summary>
    /// Gets or sets log level dictionary.
    /// </summary>
    public Dictionary<string, string> LogLevel { get; set; }
}
