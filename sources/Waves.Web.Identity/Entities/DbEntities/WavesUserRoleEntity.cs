using Microsoft.AspNetCore.Identity;

namespace Waves.Web.Identity.Entities.DbEntities
{
    /// <summary>
    /// User role database entity.
    /// </summary>
    public class WavesUserRoleEntity : IdentityRole<int>
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesUserRoleEntity"/>.
        /// </summary>
        /// <param name="role">Role name.</param>
        public WavesUserRoleEntity(string role)
            : base(role)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="WavesUserRoleEntity"/>.
        /// </summary>
        public WavesUserRoleEntity()
        {
        }
    }
}
