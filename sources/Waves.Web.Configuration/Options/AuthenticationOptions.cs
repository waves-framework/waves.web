using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Waves.Web.Configuration.Options
{
    /// <summary>
    /// Authentication options.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Gets issuer.
        /// </summary>
        public string Issuer { get; set; } = "WavesApiService";

        /// <summary>
        /// Gets audience.
        /// </summary>
        public string Audience { get; set; } = "WavesApiClient";

        /// <summary>
        /// Gets lifetime.
        /// </summary>
        public int Lifetime { get; set; } = 30;

        /// <summary>
        /// Gets token secret.
        /// </summary>
        public string TokenSecret { get; set; } = "0000000000000000";

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
