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
[Route(WavesBaseEndpoints.ControllerBaseUrl)]
public abstract class WavesUserController : WavesBaseController
{
    private readonly IWavesUserService _userService;
    private readonly IWavesTokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<WavesUserController> _logger;

    /// <summary>
    /// Creates new instance of <see cref="WavesUserController"/>.
    /// </summary>
    /// <param name="userService">User service.</param>
    /// <param name="tokenService">Token service.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="logger">Logger.</param>
    protected WavesUserController(
        IWavesUserService userService,
        IWavesTokenService tokenService,
        IMapper mapper,
        ILogger<WavesUserController> logger)
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
    [HttpPost(WavesUserControllerEndpoints.AuthenticationEndpoint)]
    [ProducesResponseType(typeof(WavesUserLoginOutputApiEntity), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public virtual async Task<IActionResult> Authenticate([FromBody] WavesUserLoginInputApiEntity userLoginApiEntity)
    {
        var loginDto = _mapper.Map<WavesUserLoginDto>(userLoginApiEntity);
        var user = await _userService.Authenticate(loginDto);
        if (user.Equals(default(WavesUserDto)))
        {
            return BadRequest(new { message = "Login or password is incorrect" });
        }

        var tokenString = await _tokenService.Get(user.Email, user.Role);
        var result = _mapper.Map<WavesUserLoginOutputApiEntity>(user);
        result.Token = tokenString;

        return Ok(result);
    }

    /// <summary>
    /// Registers new user.
    /// </summary>
    /// <param name="registrationApiEntity">User.</param>
    /// <returns>Must returns <see cref="WavesUserDto"/> if success.</returns>
    [AllowAnonymous]
    [HttpPost(WavesUserControllerEndpoints.RegistrationEndpoint)]
    [ProducesResponseType(typeof(WavesUserRegistrationOutputApiEntity), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public virtual async Task<IActionResult> Register([FromBody] WavesUserRegistrationInputApiEntity registrationApiEntity)
    {
        var registrationDto = _mapper.Map<WavesUserRegistrationDto>(registrationApiEntity);
        var userDto = await _userService.Register(registrationDto);
        var userApiEntity = _mapper.Map<WavesUserRegistrationOutputApiEntity>(userDto);
        return Ok(userApiEntity);
    }
}
