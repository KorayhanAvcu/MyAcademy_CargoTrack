using CargoTrack.Business.Services.Employees;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    [Authorize(Roles = Roles.Manager)]
    public abstract class ManagerBaseController(IEmployeeService employeeService) : BaseController
    {
        protected async Task<ResultEmployeeDto> GetCurrentEmployeeAsync()
        {
            return await employeeService.GetByAppUserIdAsync(CurrentUserId)
                   ?? throw new UnauthorizedAccessException("Bu kullanıcıya bağlı personel kaydı yok.");
        }
    }
}
