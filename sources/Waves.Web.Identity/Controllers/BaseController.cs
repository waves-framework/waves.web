using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Waves.Web.Identity.Controllers;

/// <summary>
/// Base controller.
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Tries to get user ID.
    /// </summary>
    /// <returns>User ID.</returns>
    protected int? TryGetUserId()
    {
        return int.TryParse(
            User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            out var userId)
            ? userId
            : null;
    }
}
