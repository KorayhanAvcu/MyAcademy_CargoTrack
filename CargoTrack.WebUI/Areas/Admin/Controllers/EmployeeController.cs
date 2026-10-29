using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.Users;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class EmployeeController(IEmployeeService _employeeService,
                                    IBranchService _branchService,
                                    IUserService _userService) : BaseController
    {
        public async Task<IActionResult> Index(string? search, Guid? branchId, int page = 1)
        {
            ViewBag.Search = search;
            ViewBag.BranchId = branchId;
            await LoadBranchesAsync();
            return View(await _employeeService.GetPagedAsync(search, branchId, page, 10));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectsAsync();
                return View(dto);
            }
            await _employeeService.CreateAsync(dto);
            Success("Personel eklendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await LoadBranchesAsync();
            return View(await _employeeService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadBranchesAsync();
                return View(dto);
            }
            await _employeeService.UpdateAsync(dto);
            Success("Personel güncellendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _employeeService.DeleteAsync(id);
            Success("Personel silindi.");
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadBranchesAsync()
        {
            ViewBag.Branches = (await _branchService.GetAllAsync())
                .Select(b => new SelectListItem($"{b.City.Name} / {b.Name}", b.Id.ToString()))
                .ToList();
        }

        private async Task LoadSelectsAsync()
        {
            await LoadBranchesAsync();
            ViewBag.Users = (await _userService.GetByRoleAsync(Roles.Manager))
                .Select(u => new SelectListItem($"{u.FullName} ({u.UserName})", u.Id.ToString()))
                .ToList();
        }
    }
}
