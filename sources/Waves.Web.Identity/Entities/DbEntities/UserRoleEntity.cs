using Microsoft.AspNetCore.Identity;

namespace Waves.Web.Identity.Entities.DbEntities
{
    /// <summary>
    /// User role database entity.
    /// </summary>
    public class UserRoleEntity : IdentityRole<int>
    {
        /// <summary>
        /// Creates new instance of <see cref="UserRoleEntity"/>.
        /// </summary>
        /// <param name="role">Role name.</param>
        public UserRoleEntity(string role)
            : base(role)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="UserRoleEntity"/>.
        /// </summary>
        public UserRoleEntity()
        {
        }
    }
}
