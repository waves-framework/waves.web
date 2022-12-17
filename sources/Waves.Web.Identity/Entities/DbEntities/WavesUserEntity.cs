using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Waves.Web.Identity.Entities.DbEntities
{
    /// <summary>
    /// User database entity.
    /// </summary>
    public class WavesUserEntity :
        IdentityUser<int>,
        IEquatable<WavesUserEntity>
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesUserEntity"/>.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="email">Mail.</param>
        /// <param name="passwordHash">Password hash.</param>
        /// <param name="role">Role.</param>
        public WavesUserEntity(
            string userName,
            string email,
            string passwordHash,
            string role)
            : base(userName)
        {
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets user role.
        /// </summary>
        [Required]
        public string Role { get; set; }

        /// <summary>
        /// Gets ot sets image URL.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <inheritdoc />
        public bool Equals(WavesUserEntity? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return FirstName == other.FirstName
                   && LastName == other.LastName
                   && Role == other.Role
                   && Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj)
                   || (obj is WavesUserEntity other && Equals(other));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, Role, Id);
        }
    }
}
