namespace Waves.Web.Identity.Entities.ApiEntities
{
    /// <summary>
    /// User login API input entity.
    /// </summary>
    public class UserLoginInputApiEntity
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password hash.
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
