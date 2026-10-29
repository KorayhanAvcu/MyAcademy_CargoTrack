using CargoTrack.Business.Services.Cities;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.TransferCenterDtos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class TransferCenterController(ITransferCenterService _transferCenterService,
                                          ICityService _cityService) : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return View(await _transferCenterService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCitiesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransferCenterDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCitiesAsync();
                return View(dto);
            }
            await _transferCenterService.CreateAsync(dto);
            Success("Transfer merkezi eklendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await LoadCitiesAsync();
            return View(await _transferCenterService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTransferCenterDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCitiesAsync();
                return View(dto);
            }
            await _transferCenterService.UpdateAsync(dto);
            Success("Transfer merkezi güncellendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _transferCenterService.DeleteAsync(id);
            Success("Transfer merkezi silindi.");
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCitiesAsync()
        {
            ViewBag.Cities = (await _cityService.GetAllAsync())
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();
        }
    }
}
