using System.Text.RegularExpressions;

namespace Waves.Web.Identity.Utils;

/// <summary>
/// Regex utils for splitting strings.
/// </summary>
public static class SplitStringUtils
{
    /// <summary>
    /// Gets IP address and port from input string.
    /// </summary>
    /// <param name="input">Input string.</param>
    /// <returns>Returns IP address and port.</returns>
    public static (string, string) GetIpAndPort(string input)
    {
        var pattern = @"((?::))(?:[0-9]+)$";
        var match = Regex.Match(input, pattern);
        var matchStr = match.Groups[0].Value;
        var ip = input.Replace(matchStr, string.Empty);
        var port = matchStr.Replace(":", string.Empty);
        return (ip, port);
    }
}
