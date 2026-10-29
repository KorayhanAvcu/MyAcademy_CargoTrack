using CargoTrack.DTO.DTOs.Cargos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class CargoDetailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CargoDetailDto cargo) => View(cargo);
    }
}
