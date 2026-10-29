using CargoTrack.Business.Services.Users;
using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class RoleAssignController(IUserService _userService) : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetAllWithRolesAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid id)
        {
            var (fullName, roles) = await _userService.GetRoleAssignAsync(id);
            ViewBag.fullName = fullName;
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(List<RoleAssignDto> model)
        {
            await _userService.AssignRolesAsync(model);
            Success("Roller güncellendi.");
            return RedirectToAction(nameof(Index));
        }
    }
}