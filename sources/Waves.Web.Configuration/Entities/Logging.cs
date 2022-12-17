using System.Collections.Generic;

namespace Waves.Web.Configuration.Entities;

/// <summary>
/// Logging configuration entity.
/// </summary>
public class Logging
{
    /// <summary>
    /// Gets or sets log level dictionary.
    /// </summary>
    public Dictionary<string, string> LogLevel { get; set; }
}
