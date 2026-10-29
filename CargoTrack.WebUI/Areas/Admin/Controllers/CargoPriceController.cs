using CargoTrack.Business.Services.CargoPrices;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class CargoPriceController(ICargoPriceService _cargoPriceService) : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var prices = await _cargoPriceService.GetAllAsync();
            return View(prices.OrderBy(p => p.DeliveryType)
                              .ThenBy(p => p.DistanceType)
                              .ThenBy(p => p.MinDesi)
                              .ToList());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateCargoPriceDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            await _cargoPriceService.CreateAsync(dto);
            Success("Fiyat kuralı eklendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id) => View(await _cargoPriceService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCargoPriceDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            await _cargoPriceService.UpdateAsync(dto);
            Success("Fiyat kuralı güncellendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cargoPriceService.DeleteAsync(id);
            Success("Fiyat kuralı silindi.");
            return RedirectToAction(nameof(Index));
        }
    }
}
