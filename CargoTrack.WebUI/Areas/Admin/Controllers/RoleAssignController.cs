using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class RoleAssignController(UserManager<AppUser> _userManager,RoleManager<AppRole> _roleManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var mappedUsers = users.Adapt<List<ResultUserDto>>();
            return View(mappedUsers);
        }

        public async Task<IActionResult> GetUserForRoleAssign(Guid id)
        {
            //var user = await _userManager.FindByIdAsync(id.ToString());
            //var roles = await _roleManager.Roles.ToListAsync();
            //var userRoles = await _userManager.GetRolesAsync(user);

            //var roleAssignList = new List<RoleAssignDto>();
            //foreach (var role in userRoles)
            //{

            //}
            return View();

        }
    }
}
