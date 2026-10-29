using CargoTrack.Business.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    public class DashboardController(IEmployeeService employeeService) : ManagerBaseController(employeeService)
    {
        public async Task<IActionResult> Index()
        {
            var employee = await GetCurrentEmployeeAsync();
            ViewBag.BranchId = employee.BranchId;
            ViewBag.BranchName = $"{employee.BranchCityName} / {employee.BranchName}";
            return View();
        }
    }
}