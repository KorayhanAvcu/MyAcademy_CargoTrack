using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Controllers
{
    public class ErrorPagesController : Controller
    {
        public IActionResult AccessDenied() => View();
        public IActionResult Error() => View();

        public IActionResult Status(int code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}
