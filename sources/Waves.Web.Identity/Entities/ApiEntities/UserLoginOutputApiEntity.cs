namespace Waves.Web.Identity.Entities.ApiEntities
{
    /// <summary>
    /// Out API model for user login.
    /// </summary>
    public class UserLoginOutputApiEntity
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets user role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets user token.
        /// </summary>
        public string Token { get; set; }
    }
}
