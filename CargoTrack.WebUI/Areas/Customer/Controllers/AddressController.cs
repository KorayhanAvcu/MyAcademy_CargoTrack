using CargoTrack.Business.Services.Addresses;
using CargoTrack.Business.Services.Cities;
using CargoTrack.DTO.DTOs.AddressDtos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Customer.Controllers
{
    [Area(Area.Customer)]
    [Authorize(Roles = Roles.User)]
    public class AddressController(IAddressService _addressService, ICityService _cityService) : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return View(await _addressService.GetByUserAsync(CurrentUserId));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCitiesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAddressDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCitiesAsync();
                return View(dto);
            }
            await _addressService.CreateAsync(dto, CurrentUserId);
            Success("Adres eklendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await LoadCitiesAsync();
            return View(await _addressService.GetForUpdateAsync(id, CurrentUserId));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAddressDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCitiesAsync();
                return View(dto);
            }
            await _addressService.UpdateAsync(dto, CurrentUserId);
            Success("Adres güncellendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _addressService.DeleteAsync(id, CurrentUserId);
            Success("Adres silindi.");
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
