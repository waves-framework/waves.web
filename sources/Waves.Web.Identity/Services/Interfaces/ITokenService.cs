using System.Threading.Tasks;

namespace Waves.Web.Identity.Services.Interfaces
{
    /// <summary>
    /// Interface for token service.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gets token for current user and role.
        /// </summary>
        /// <param name="identifier">User identifier.</param>
        /// <param name="role">User role.</param>
        /// <returns>Token.</returns>
        Task<string> Get(string identifier, string role);
    }
}
