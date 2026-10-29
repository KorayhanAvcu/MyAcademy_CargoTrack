using CargoTrack.Business.Services.Cargos;
using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Customer.Controllers
{
    [Area(Area.Customer)]
    [Authorize(Roles = Roles.User)]
    public class CargoController(ICargoService _cargoService) : BaseController
    {
        public async Task<IActionResult> Index(CargoFilterDto filter)
        {
            ViewBag.Filter = filter;
            return View(await _cargoService.GetByUserAsync(CurrentUserId, filter));
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            return View(await _cargoService.GetDetailAsync(id, CurrentUserId, false, null));
        }
    }
}
