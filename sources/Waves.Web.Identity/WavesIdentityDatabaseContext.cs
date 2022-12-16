using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Waves.Web.Identity.Entities.DbEntities;

namespace Waves.Web.Identity;

/// <summary>
/// Identity database context.
/// </summary>
/// <typeparam name="TContext">Context type.</typeparam>
/// <typeparam name="TUser">Type of user.</typeparam>
/// <typeparam name="TRole">Type of role.</typeparam>
public abstract class WavesIdentityDatabaseContext<TContext, TUser, TRole> :
    IdentityDbContext<UserEntity, UserRoleEntity, int>
    where TContext : WavesIdentityDatabaseContext<TContext, TUser, TRole>
    where TUser : UserEntity
    where TRole : UserRoleEntity
{
    /// <summary>
    /// Creates new instance of <see cref="T"/>.
    /// </summary>
    /// <param name="options">Options.</param>
    protected WavesIdentityDatabaseContext(DbContextOptions<TContext> options)
        : base(options)
    {
    }
}
