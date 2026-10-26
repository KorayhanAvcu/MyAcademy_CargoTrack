using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Controllers
{
    public class LoginController(SignInManager<AppUser>  _signInManager,
                                UserManager<AppUser> _userManager) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginUserDto loginUserDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUserDto.UserName, loginUserDto.Password,false,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                return View(loginUserDto);
            }
            var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
            var userRoles = await _userManager.GetRolesAsync(user);
            if(userRoles.Contains(Roles.Admin))
            {
                return Redirect("/Admin/Branch/Index");
            }

            if (userRoles.Contains(Roles.Manager))
            {
                return Redirect("/Manager/Dashboard");
            }
            if(userRoles.Contains(Roles.User))
            {
                return Redirect("/User/Dashboard/Index");
            }
            return RedirectToAction("Index", "Home");   

        }
    }

    
}
