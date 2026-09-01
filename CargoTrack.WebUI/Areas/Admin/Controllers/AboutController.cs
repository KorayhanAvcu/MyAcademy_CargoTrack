using CargoTrack.Business.Services.Abouts;
using CargoTrack.DTO.DTOs.AboutDtos;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class AboutController(IAboutService _aboutService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var abouts = await _aboutService.GetAllAsync();
            return View(abouts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAboutDto aboutDto)
        {
            if (!ModelState.IsValid)
            {
                return View(aboutDto);
            }

            await _aboutService.CreateAsync(aboutDto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var about = await _aboutService.GetByIdAsync(id);
            return View(about);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAboutDto aboutDto)
        {
            if (!ModelState.IsValid)
            {
                return View(aboutDto);
            }
            await _aboutService.UpdateAsync(aboutDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _aboutService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}