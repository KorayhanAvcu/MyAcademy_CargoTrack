using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    public class CargoController(ICargoService _cargoService,
                                 IBranchService _branchService,
                                 ITransferCenterService _transferCenterService,
                                 IEmployeeService employeeService) : ManagerBaseController(employeeService)
    {
        public async Task<IActionResult> Index(CargoFilterDto filter)
        {
            var employee = await GetCurrentEmployeeAsync();
            ViewBag.Filter = filter;
            return View(await _cargoService.GetByBranchAsync(employee.BranchId, filter));
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var employee = await GetCurrentEmployeeAsync();
            return View(await _cargoService.GetDetailAsync(id, CurrentUserId, false, employee.BranchId));
        }

        public async Task<IActionResult> DeliveryList(CargoFilterDto filter)
        {
            var employee = await GetCurrentEmployeeAsync();
            filter.Status = CargoStatus.OutForDelivery;
            return View(await _cargoService.GetByBranchAsync(employee.BranchId, filter));
        }

        public async Task<IActionResult> Problematic()
        {
            var employee = await GetCurrentEmployeeAsync();
            var branchId = employee.BranchId;

            ViewBag.Failed = (await _cargoService.GetByBranchAsync(branchId,
                new CargoFilterDto { Status = CargoStatus.DeliveryFailed, PageSize = 50 })).Items;
            ViewBag.Returning = (await _cargoService.GetByBranchAsync(branchId,
                new CargoFilterDto { Status = CargoStatus.ReturnInProgress, PageSize = 50 })).Items;
            ViewBag.Delayed = (await _cargoService.GetByBranchAsync(branchId,
                new CargoFilterDto { OnlyDelayed = true, PageSize = 50 })).Items;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Movement(Guid id)
        {
            var employee = await EnsureBranchCargoAsync(id);
            await LoadMovementSelectsAsync();
            return View(new ChangeCargoStatusDto { CargoId = id, BranchId = employee.BranchId });
        }

        [HttpPost]
        public async Task<IActionResult> Movement(ChangeCargoStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadMovementSelectsAsync();
                return View(dto);
            }

            var employee = await GetCurrentEmployeeAsync();
            await _cargoService.ChangeStatusAsync(dto, CurrentUserId, employee.Id);
            Success("Kargo hareketi kaydedildi.");
            return RedirectToAction(nameof(Detail), new { id = dto.CargoId });
        }

        [HttpGet]
        public async Task<IActionResult> Deliver(Guid id)
        {
            await EnsureBranchCargoAsync(id);
            return View(new DeliverCargoDto { CargoId = id });
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(DeliverCargoDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var employee = await GetCurrentEmployeeAsync();
            await _cargoService.DeliverAsync(dto, CurrentUserId, employee.Id);
            Success("Kargo teslim edildi.");
            return RedirectToAction(nameof(Detail), new { id = dto.CargoId });
        }

        [HttpGet]
        public async Task<IActionResult> ReportFailure(Guid id)
        {
            await EnsureBranchCargoAsync(id);
            return View(new ReportDeliveryFailureDto { CargoId = id });
        }

        [HttpPost]
        public async Task<IActionResult> ReportFailure(ReportDeliveryFailureDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var employee = await GetCurrentEmployeeAsync();
            await _cargoService.ReportFailureAsync(dto, CurrentUserId, employee.Id);
            Success("Başarısız teslimat kaydedildi.");
            return RedirectToAction(nameof(Detail), new { id = dto.CargoId });
        }

        private async Task<ResultEmployeeDto> EnsureBranchCargoAsync(Guid cargoId)
        {
            var employee = await GetCurrentEmployeeAsync();
            await _cargoService.GetDetailAsync(cargoId, CurrentUserId, false, employee.BranchId);
            return employee;
        }

        private async Task LoadMovementSelectsAsync()
        {
            var hidden = new[] { CargoStatus.Created, CargoStatus.Delivered, CargoStatus.DeliveryFailed };
            ViewBag.Statuses = Enum.GetValues<CargoStatus>()
                .Where(s => !hidden.Contains(s))
                .Select(s => new SelectListItem(s.ToDisplayName(), s.ToString()))
                .ToList();

            ViewBag.Branches = (await _branchService.GetAllAsync())
                .Select(b => new SelectListItem(b.Name, b.Id.ToString()))
                .ToList();

            ViewBag.TransferCenters = (await _transferCenterService.GetAllAsync())
                .Select(t => new SelectListItem($"{t.Name} ({t.CityName})", t.Id.ToString()))
                .ToList();
        }
    }
}
