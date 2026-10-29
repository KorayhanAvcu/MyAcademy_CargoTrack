using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.ViewComponents
{
    public class CargoFilterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CargoFilterDto filter, bool showDirection)
        {
            ViewBag.Statuses = Enum.GetValues<CargoStatus>()
                .Select(s => new SelectListItem(s.ToDisplayName(), s.ToString(), s == filter.Status))
                .ToList();

            ViewBag.Directions = Enum.GetValues<CargoDirection>()
                .Select(d => new SelectListItem(d.ToDisplayName(), d.ToString(), d == filter.Direction))
                .ToList();

            ViewBag.ShowDirection = showDirection;
            return View(filter);
        }
    }
}
