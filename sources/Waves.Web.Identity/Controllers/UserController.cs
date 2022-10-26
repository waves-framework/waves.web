using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Waves.Web.Identity.Endpoints;
using Waves.Web.Identity.Entities.ApiEntities;
using Waves.Web.Identity.Entities.DtoEntities;
using Waves.Web.Identity.Services.Interfaces;

namespace Waves.Web.Identity.Controllers;

/// <summary>
/// Users controller.
/// </summary>
[ApiController]
[Route(BaseEndpoints.ControllerBaseUrl)]
public abstract class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;

    /// <summary>
    /// Creates new instance of <see cref="UserController"/>.
    /// </summary>
    /// <param name="userService">User service.</param>
    /// <param name="tokenService">Token service.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="logger">Logger.</param>
    protected UserController(
        IUserService userService,
        ITokenService tokenService,
        IMapper mapper,
        ILogger<UserController> logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Authenticate user.
    /// </summary>
    /// <param name="userLoginApiEntity">User login API entity.</param>
    /// <returns>Must returns user token if success.</returns>
    [AllowAnonymous]
    [HttpPost(UserControllerEndpoints.AuthenticationEndpoint)]
    [ProducesResponseType(typeof(UserLoginOutputApiEntity), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public virtual async Task<IActionResult> Authenticate([FromBody] UserLoginInputApiEntity userLoginApiEntity)
    {
        var loginDto = _mapper.Map<UserLoginDto>(userLoginApiEntity);
        var user = await _userService.Authenticate(loginDto);
        if (user.Equals(default(UserDto)))
        {
            return BadRequest(new { message = "Login or password is incorrect" });
        }

        var tokenString = await _tokenService.Get(user.Email, user.Role);
        var result = _mapper.Map<UserLoginOutputApiEntity>(user);
        result.Token = tokenString;

        return Ok(result);
    }

    /// <summary>
    /// Registers new user.
    /// </summary>
    /// <param name="registrationApiEntity">User.</param>
    /// <returns>Must returns <see cref="UserDto"/> if success.</returns>
    [AllowAnonymous]
    [HttpPost(UserControllerEndpoints.RegistrationEndpoint)]
    [ProducesResponseType(typeof(UserRegistrationOutputApiEntity), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public virtual async Task<IActionResult> Register([FromBody] UserRegistrationInputApiEntity registrationApiEntity)
    {
        var registrationDto = _mapper.Map<UserRegistrationDto>(registrationApiEntity);
        var userDto = await _userService.Register(registrationDto);
        var userApiEntity = _mapper.Map<UserRegistrationOutputApiEntity>(userDto);
        return Ok(userApiEntity);
    }
}
