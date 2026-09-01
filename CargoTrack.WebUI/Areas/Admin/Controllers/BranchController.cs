using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cities;
using CargoTrack.DTO.DTOs.BranchDtos;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class BranchController(IBranchService _branchService,
                                  ICityService _cityService) : Controller
    {
        private async Task GetCitiesAsync()
        {
            var cities = await _cityService.GetAllAsync();

            var sortedCities = cities.OrderBy(c => c.Name).ToList();
            ViewBag.Cities = (from city in sortedCities
                              select new SelectListItem
                              {
                                  Text = city.Name,
                                  Value = city.Id.ToString()
                              }).ToList();
        }
        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetAllAsync();
            return View(branches);
        }

        public async Task<IActionResult> Create()
        {
            await GetCitiesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBranchDto branchDto)
        {
            if (!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(branchDto);
            }
            await _branchService.CreateAsync(branchDto);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(Guid id)
        {
            await GetCitiesAsync();
            var branch = await _branchService.GetByIdAsync(id);
            return View(branch);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBranchDto branchDto)
        {
            if (!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(branchDto);
            }
            await _branchService.UpdateAsync(branchDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _branchService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
