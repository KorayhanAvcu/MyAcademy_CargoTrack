using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.DTO.DTOs.AuditLogDtos;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class AuditLogController(IAuditLogService _auditLogService) : BaseController
    {
        public async Task<IActionResult> Index(AuditLogFilterDto filter)
        {
            ViewBag.Filter = filter;
            return View(await _auditLogService.GetPagedAsync(filter));
        }
    }
}