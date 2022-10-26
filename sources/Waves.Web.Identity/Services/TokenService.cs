using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Waves.Web.Configuration.Services.Interfaces;
using Waves.Web.Identity.Security.Options;
using Waves.Web.Identity.Services.Interfaces;

namespace Waves.Web.Identity.Services
{
    /// <summary>
    /// Token service.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly string _securityKey;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly DateTimeOffset _tokenLifetime;

        /// <summary>
        /// Creates new instance of <see cref="TokenService"/>.
        /// </summary>
        /// <param name="configurationService">Configuration.</param>
        public TokenService(IConfigurationService configurationService)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenLifetime = DateTime.UtcNow.AddDays(AuthenticationOptions.Lifetime);
            _securityKey = configurationService.GetTokenSecret();
        }

        /// <inheritdoc />
        public Task<string> Get(string identifier, string role)
        {
            if (string.IsNullOrWhiteSpace(_securityKey))
            {
                throw new AuthenticationException("Main security key hasn't initialized.");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, identifier),
                    new Claim(ClaimTypes.Role, role),
                }),
                Issuer = AuthenticationOptions.Issuer,
                Audience = AuthenticationOptions.Audience,
                Expires = _tokenLifetime.DateTime,
                SigningCredentials = new SigningCredentials(
                    AuthenticationOptions.GetSymmetricSecurityKey(_securityKey),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = _tokenHandler.WriteToken(token);

            return Task.FromResult(tokenString);
        }
    }
}
