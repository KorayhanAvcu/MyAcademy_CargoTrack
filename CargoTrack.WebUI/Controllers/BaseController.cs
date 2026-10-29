using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CargoTrack.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        protected void Success(string message) => TempData["success"] = message;
    }
}
