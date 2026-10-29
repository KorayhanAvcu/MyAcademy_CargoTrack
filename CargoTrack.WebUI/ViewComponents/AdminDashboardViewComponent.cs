using CargoTrack.Business.Services.Dashboards;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class AdminDashboardViewComponent(IDashboardService _dashboardService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _dashboardService.GetAdminDashboardAsync());
        }
    }
}
