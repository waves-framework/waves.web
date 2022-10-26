using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Waves.Web.Identity.Entities.DbEntities;

namespace Waves.Web.Identity;

/// <summary>
/// Identity database context.
/// </summary>
/// <typeparam name="T">Type of database context.</typeparam>
public abstract class WavesIdentityDatabaseContext<T> :
    IdentityDbContext<UserDbEntity, UserRoleDbEntity, int>
    where T : WavesIdentityDatabaseContext<T>
{
    /// <summary>
    /// Creates new instance of <see cref="T"/>.
    /// </summary>
    /// <param name="options">Options.</param>
    protected WavesIdentityDatabaseContext(DbContextOptions<T> options)
        : base(options)
    {
    }
}
