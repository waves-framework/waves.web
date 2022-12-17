using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Entities.DtoEntities;
using Waves.Web.Identity.Services.Interfaces;

namespace Waves.Web.Identity.Services;

/// <summary>
/// User service.
/// </summary>
/// <typeparam name="TContext">Context type.</typeparam>
/// <typeparam name="TUser">Type of user.</typeparam>
/// <typeparam name="TRole">Type of role.</typeparam>
public class UserService<TContext, TUser, TRole> : IUserService
    where TContext : WavesIdentityDatabaseContext<TContext, TUser, TRole>
    where TUser : UserEntity
    where TRole : UserRoleEntity
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly TContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService<TContext, TUser, TRole>> _logger;

    /// <summary>
    /// Creates new instance of <see cref="UserService{T}"/>.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="databaseContext">Database context.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="logger">Logger.</param>
    public UserService(
        UserManager<UserEntity> userManager,
        TContext databaseContext,
        IMapper mapper,
        ILogger<UserService<TContext, TUser, TRole>> logger)
    {
        _userManager = userManager;
        _context = databaseContext;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<UserDto> Register(UserRegistrationDto user)
    {
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            throw new ArgumentException("Password not set.");
        }

        if (_context.Users.Any(x => x.UserName == user.Username))
        {
            throw new ArgumentException("User with same login yet exist.");
        }

        var entity = _mapper.Map<UserEntity>(user);

        await _userManager.CreateAsync(entity);
        await _userManager.AddToRoleAsync(entity, user.Role);

        return _mapper.Map<UserDto>(entity);
    }

    /// <inheritdoc />
    public async Task<UserDto> Authenticate(UserLoginDto login)
    {
        if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.PasswordHash))
        {
            throw new AuthenticationException("Empty email or password.");
        }

        var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == login.Email)
                   ?? await _context.Users.SingleOrDefaultAsync(x => x.UserName == login.Email);

        if (user == null)
        {
            throw new AuthenticationException("User not found.");
        }

        if (user.PasswordHash != login.PasswordHash)
        {
            throw new AuthenticationException("Password is incorrect.");
        }

        return _mapper.Map<UserDto>(user);
    }

    /// <inheritdoc />
    public async Task<UserDto?> Get(int id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id.Equals(id));
        return user == null ? default : _mapper.Map<UserDto>(user);
    }

    /// <inheritdoc />
    public async Task<UserDto?> Get(string email)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));
        return user == null ? default : _mapper.Map<UserDto>(user);
    }
}
