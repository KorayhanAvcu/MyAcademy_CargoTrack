using CargoTrack.Business.Services.Reports;
using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class ReportController(IReportService _reportService) : BaseController
    {
        public async Task<IActionResult> Index() => View(await _reportService.GetReportAsync());
    }
}