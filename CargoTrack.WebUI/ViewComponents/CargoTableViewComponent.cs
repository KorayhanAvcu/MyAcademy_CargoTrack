using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class CargoTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<ResultCargoDto> cargos)
        {
            var today = DateTime.Today;
            var rows = cargos.Select(c => new CargoRowViewModel(c,
                c.EstimatedArrivalDate < today &&
                c.CargoStatus != CargoStatus.Delivered &&
                c.CargoStatus != CargoStatus.ReturnedToSender))
                .ToList();

            return View(rows);
        }
    }
}
