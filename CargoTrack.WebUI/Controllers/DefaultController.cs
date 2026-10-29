using CargoTrack.Business.Services.Cargos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Controllers
{
    public class DefaultController(ICargoService _cargoService) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Error = TempData["error"];
            return View();
        }

        public async Task<IActionResult> CargoDetails(string trackCode)
        {
            var cargo = await _cargoService.GetByTrackCodeAsync(trackCode);
            if (cargo == null)
            {
                TempData["error"] = "Bu takip numarasına ait bir kargo bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
        }
    }
}
