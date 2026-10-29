using CargoTrack.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class AlertsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new AlertViewModel(TempData["error"] as string, TempData["success"] as string);
            return View(model);
        }
    }
}
