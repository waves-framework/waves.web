namespace Waves.Web.Identity.Entities.DtoEntities
{
    /// <summary>
    /// User dto.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public int Id { get; set; }

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

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Username} (email: {Email}, id: {Id}, role {Role})";
        }
    }
}
