using System.Threading.Tasks;
using Waves.Web.Identity.Entities.DtoEntities;

namespace Waves.Web.Identity.Services.Interfaces
{
    /// <summary>
    /// Interface for user service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers new user.
        /// </summary>
        /// <param name="user">User registration DTO.</param>
        /// <returns>User DTO.</returns>
        Task<UserDto> Register(UserRegistrationDto user);

        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="login">User login DTO.</param>
        /// <returns>User DTO.</returns>
        Task<UserDto> Authenticate(UserLoginDto login);

        /// <summary>
        /// Gets user by ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>User DTO.</returns>
        Task<UserDto?> Get(int id);

        /// <summary>
        /// Gets user by email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>User DTO.</returns>
        Task<UserDto?> Get(string email);
    }
}
