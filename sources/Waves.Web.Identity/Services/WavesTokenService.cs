using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Waves.Web.Configuration.Options;
using Waves.Web.Configuration.Services.Interfaces;
using Waves.Web.Identity.Services.Interfaces;

namespace Waves.Web.Identity.Services
{
    /// <summary>
    /// Token service.
    /// </summary>
    public class WavesTokenService : IWavesTokenService
    {
        private readonly string _securityKey;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly DateTimeOffset _tokenLifetime;
        private readonly AuthenticationOptions _options;

        /// <summary>
        /// Creates new instance of <see cref="WavesTokenService"/>.
        /// </summary>
        /// <param name="configurationService">Configuration.</param>
        public WavesTokenService(IConfigurationService configurationService)
        {
            _options = configurationService.GetAuthentication();
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenLifetime = DateTime.UtcNow.AddDays(_options.Lifetime);
            _securityKey = _options.TokenSecret;
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
                Issuer = _options.Issuer,
                Audience = _options.Audience,
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
