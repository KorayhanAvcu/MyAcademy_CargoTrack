using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Customer.Controllers
{
    [Area(Area.Customer)]
    [Authorize(Roles = Roles.User)]
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.UserId = CurrentUserId;
            return View();
        }
    }
}