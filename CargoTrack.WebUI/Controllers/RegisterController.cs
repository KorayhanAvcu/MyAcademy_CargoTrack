using CargoTrack.Business.Services.Users;
using CargoTrack.DTO.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Controllers
{
    public class RegisterController(IUserService _userService) : Controller
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

            var result = await _userService.RegisterAsync(registerUserDto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registerUserDto);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}