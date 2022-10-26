using System.Collections.Generic;

namespace Waves.Web.Identity.Security
{
    /// <summary>
    /// Roles.
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Gets administrator role.
        /// </summary>
        public const string AdministratorRole = "Administrator";

        /// <summary>
        /// Gets support role.
        /// </summary>
        public const string SupportRole = "Support";

        /// <summary>
        /// Gets service role.
        /// </summary>
        public const string ServiceRole = "Service";

        /// <summary>
        /// Gets user role.
        /// </summary>
        public const string UserRole = "User";

        /// <summary>
        /// Gets roles.
        /// </summary>
        /// <returns>Roles.</returns>
        public static IEnumerable<string> Get()
        {
            return new[]
            {
                AdministratorRole,
                SupportRole,
                ServiceRole,
                UserRole
            };
        }
    }
}
