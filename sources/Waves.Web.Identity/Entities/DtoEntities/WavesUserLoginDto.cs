namespace Waves.Web.Identity.Entities.DtoEntities
{
    /// <summary>
    /// User login DTO.
    /// </summary>
    public class WavesUserLoginDto
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
