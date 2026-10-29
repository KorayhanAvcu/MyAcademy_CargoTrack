using CargoTrack.Business.Services.Addresses;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Users;
using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using CargoTrack.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class CargoController(ICargoService _cargoService,
                                 IBranchService _branchService,
                                 IUserService _userService,
                                 IAddressService _addressService) : BaseController
    {
        public async Task<IActionResult> Index(CargoFilterDto filter)
        {
            ViewBag.Filter = filter;
            return View(await _cargoService.GetPagedAsync(filter));
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            return View(await _cargoService.GetDetailAsync(id, CurrentUserId, true, null));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCargoDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectsAsync();
                return View(dto);
            }

            var trackCode = await _cargoService.CreateAsync(dto, CurrentUserId);
            Success($"Kargo oluşturuldu. Takip numarası: {trackCode}");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ReceiverAddresses(Guid receiverId)
        {
            var addresses = await _addressService.GetByUserAsync(receiverId);
            return Json(addresses.Select(a => new { id = a.Id, text = a.FullText, cityId = a.CityId }));
        }

        private async Task LoadSelectsAsync()
        {
            ViewBag.Customers = (await _userService.GetCustomersAsync())
                .Select(u => new SelectListItem(u.FullName, u.Id.ToString()))
                .ToList();

            var branches = (await _branchService.GetAllAsync())
                .OrderBy(b => b.City.Name).ThenBy(b => b.Name)
                .ToList();

            ViewBag.OriginBranches = branches
                .Select(b => new SelectListItem($"{b.City.Name} / {b.Name}", b.Id.ToString()))
                .ToList();

            ViewBag.DestinationBranches = branches
                .Select(b => new BranchOptionViewModel(b.Id, $"{b.City.Name} / {b.Name}", b.City.Id))
                .ToList();
        }
    }
}
