namespace Waves.Web.Configuration.Entities;

/// <summary>
/// Authentication entity.
/// </summary>
public class AuthenticationEntity
{
    /// <summary>
    /// Gets or sets token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets secret.
    /// </summary>
    public string Secret { get; set; }
}
