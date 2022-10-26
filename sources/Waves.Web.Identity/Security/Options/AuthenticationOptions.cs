using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Waves.Web.Identity.Security.Options
{
    /// <summary>
    /// Authentication options.
    /// </summary>
    public static class AuthenticationOptions
    {
        /// <summary>
        /// Gets issuer.
        /// </summary>
        public const string Issuer = "WavesApiService";

        /// <summary>
        /// Gets audience.
        /// </summary>
        public const string Audience = "WavesApiClient";

        /// <summary>
        /// Gets lifetime.
        /// </summary>
        public const int Lifetime = 30;

        /// <summary>
        /// Gets symmetric security key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Returns <see cref="SymmetricSecurityKey"/>.</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}
