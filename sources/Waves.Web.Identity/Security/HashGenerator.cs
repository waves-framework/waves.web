using System.Security.Cryptography;
using System.Text;

namespace Waves.Web.Identity.Security;

/// <summary>
/// Hash generator.
/// </summary>
public static class HashGenerator
{
    /// <summary>
    /// Generates SHA256 from string input.
    /// </summary>
    /// <param name="input">Input value.</param>
    /// <returns>String hash.</returns>
    public static string ToSha256(this string input)
    {
        return GenerateSha256(input);
    }

    /// <summary>
    /// Generates SHA256 from string input.
    /// </summary>
    /// <param name="input">Input value.</param>
    /// <returns>String hash.</returns>
    public static string GenerateSha256(string input)
    {
        using var generator = SHA256.Create();
        var bytes = Encoding.Default.GetBytes(input);
        var hash = generator.ComputeHash(bytes);
        var builder = new StringBuilder();
        foreach (var b in hash)
        {
            builder.Append(b.ToString("x2"));
        }

        var result = builder.ToString();
        return result;
    }
}
