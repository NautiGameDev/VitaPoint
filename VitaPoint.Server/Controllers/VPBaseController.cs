using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace VitaPoint.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class VPBaseController : ControllerBase
    {
        protected string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected bool IsUserAuthenticated => User.Identity?.IsAuthenticated ?? false;
    }
}
