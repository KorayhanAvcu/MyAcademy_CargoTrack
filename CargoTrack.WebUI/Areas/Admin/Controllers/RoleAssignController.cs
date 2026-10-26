using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class RoleAssignController(UserManager<AppUser> _userManager,RoleManager<AppRole> _roleManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            
            var mappedUsers = users.Adapt<List<ResultUserDto>>();
            foreach (var item in mappedUsers)
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString());
                item.Roles = await _userManager.GetRolesAsync(user);
            }
            return View(mappedUsers);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var roleAssignList = new List<RoleAssignDto>();

            ViewBag.fullName = string.Join(" " , user.FirstName, user.LastName);
            foreach (var role in roles)
            {
                roleAssignList.Add(new RoleAssignDto
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleExist = userRoles.Contains(role.Name)
                });
            }
            return View(roleAssignList);

        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(List<RoleAssignDto> model)
        {
            var userId = model.Select(x=>x.UserId).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            foreach (var assignRole in model)
            {
                if(assignRole.RoleExist)
                {
                    await _userManager.AddToRoleAsync(user, assignRole.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, assignRole.RoleName);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
