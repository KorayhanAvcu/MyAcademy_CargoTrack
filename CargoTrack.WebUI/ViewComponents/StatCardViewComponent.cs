using CargoTrack.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class StatCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string title, string value, string color)
        {
            return View(new StatCardViewModel(title, value, color));
        }
    }
}
