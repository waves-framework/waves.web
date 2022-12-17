namespace Waves.Web.Identity.Entities.ApiEntities
{
    /// <summary>
    /// User registration input API entity.
    /// </summary>
    public class WavesUserRegistrationInputApiEntity
    {
        /// <summary>
        /// Gets or sets username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get or sets password hash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets user role.
        /// </summary>
        public string Role { get; set; }
    }
}
