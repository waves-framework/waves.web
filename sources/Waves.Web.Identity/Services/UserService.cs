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

namespace Waves.Web.Identity.Services
{
    /// <summary>
    /// User service.
    /// </summary>
    /// <typeparam name="T">Type of database context.</typeparam>
    public class UserService<T> : IUserService
        where T : WavesIdentityDatabaseContext<T>
    {
        private readonly UserManager<UserDbEntity> _userManager;
        private readonly T _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService<T>> _logger;

        /// <summary>
        /// Creates new instance of <see cref="UserService{T}"/>.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="databaseContext">Database context.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="logger">Logger.</param>
        public UserService(
            UserManager<UserDbEntity> userManager,
            T databaseContext,
            IMapper mapper,
            ILogger<UserService<T>> logger)
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

            var entity = _mapper.Map<UserDbEntity>(user);

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
}
