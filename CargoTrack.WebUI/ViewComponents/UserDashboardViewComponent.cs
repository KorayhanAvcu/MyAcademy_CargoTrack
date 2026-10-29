using CargoTrack.Business.Services.Dashboards;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class UserDashboardViewComponent(IDashboardService _dashboardService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Guid userId)
        {
            return View(await _dashboardService.GetUserDashboardAsync(userId));
        }
    }
}
