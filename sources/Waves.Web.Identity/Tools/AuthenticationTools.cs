using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Waves.Web.Identity.Entities.DtoEntities;
using Waves.Web.Identity.Services.Interfaces;

namespace Waves.Web.Identity.Tools
{
    /// <summary>
    /// Authentication middleware tools.
    /// </summary>
    public static class AuthenticationTools
    {
        /// <summary>
        /// Authenticates user by token.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Authenticate(TokenValidatedContext context)
        {
            var userPrincipal = context.Principal;
            if (userPrincipal is { Identity: ClaimsIdentity userIdentity } && userIdentity.HasClaim(c => c.Type == userIdentity.RoleClaimType))
            {
                var userService = context.HttpContext.RequestServices.GetRequiredService<IWavesUserService>();
                if (userIdentity.Name != null)
                {
                    var user = await userService.Get(userIdentity.Name);
                    if (user.Equals(default(WavesUserDto)))
                    {
                        context.Fail("Unauthorized access.");
                        return;
                    }

                    context.Principal?.AddIdentity(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    }));

                    context.Success();
                    return;
                }
            }

            context.Fail("Identity internal error.");
        }
    }
}
