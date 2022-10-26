using Microsoft.AspNetCore.Identity;

namespace Waves.Web.Identity.Entities.DbEntities
{
    /// <summary>
    /// User role database entity.
    /// </summary>
    public class UserRoleDbEntity : IdentityRole<int>
    {
        /// <summary>
        /// Creates new instance of <see cref="UserRoleDbEntity"/>.
        /// </summary>
        /// <param name="role">Role name.</param>
        public UserRoleDbEntity(string role)
            : base(role)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="UserRoleDbEntity"/>.
        /// </summary>
        public UserRoleDbEntity()
        {
        }
    }
}
