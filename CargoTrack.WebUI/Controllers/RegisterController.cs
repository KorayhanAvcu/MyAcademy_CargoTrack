using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Controllers
{
    public class RegisterController(UserManager<AppUser> _userManager) : Controller
    {
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerUserDto);
            }

            var user = registerUserDto.Adapt<AppUser>();

            var result = await _userManager.CreateAsync(
                user,
                registerUserDto.Password
            );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registerUserDto);
            }
            await _userManager.AddToRoleAsync(user, Roles.User);
            return RedirectToAction("Index", "Login");
        }
    }
}