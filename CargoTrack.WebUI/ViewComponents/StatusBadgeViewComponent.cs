using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Extensions;
using CargoTrack.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class StatusBadgeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CargoStatus status)
        {
            var (css, icon) = status switch
            {
                CargoStatus.Delivered => ("bg-[#e8f5e9] text-[#2e7d32]", "check_circle"),
                CargoStatus.OutForDelivery => ("bg-surface-container-high text-primary-container", "local_shipping"),
                CargoStatus.InTransferCenter => ("bg-[#fff3e0] text-[#e65100]", "swap_horiz"),
                CargoStatus.DeliveryFailed => ("bg-error-container text-on-error-container", "error"),
                CargoStatus.ReturnInProgress or CargoStatus.ReturnedToSender => ("bg-[#fff3e0] text-[#e65100]", "undo"),
                CargoStatus.Created => ("bg-surface-container text-on-surface-variant", "inventory_2"),
                _ => ("bg-surface-container text-primary-container", "warehouse")
            };
            return View(new StatusBadgeViewModel(status.ToDisplayName(), css, icon));
        }
    }
}
