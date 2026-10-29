using CargoTrack.WebUI.Consts;
using CargoTrack.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var links = new List<(string Title, string Url)>();
            var user = UserClaimsPrincipal;

            if (user.IsInRole(Roles.Admin))
            {
                links.AddRange(new[]
                {
                    ("Dashboard", "/Admin/Dashboard"),
                    ("Tüm Kargolar", "/Admin/Cargo"),
                    ("Kargo Oluştur", "/Admin/Cargo/Create"),
                    ("Şubeler", "/Admin/Branch"),
                    ("Transfer Merkezleri", "/Admin/TransferCenter"),
                    ("Personel", "/Admin/Employee"),
                    ("Kullanıcı / Rol", "/Admin/RoleAssign"),
                    ("Fiyat Kuralları", "/Admin/CargoPrice"),
                    ("Audit Log", "/Admin/AuditLog"),
                    ("Raporlar", "/Admin/Report")
                });
            }

            if (user.IsInRole(Roles.Manager))
            {
                links.AddRange(new[]
                {
                    ("Dashboard", "/Manager/Dashboard"),
                    ("Şube Kargoları", "/Manager/Cargo"),
                    ("Dağıtım Listesi", "/Manager/Cargo/DeliveryList"),
                    ("Sorunlu Kargolar", "/Manager/Cargo/Problematic")
                });
            }

            if (user.IsInRole(Roles.User))
            {
                links.AddRange(new[]
                {
                    ("Dashboard", "/Customer/Dashboard"),
                    ("Kargolarım", "/Customer/Cargo"),
                    ("Adreslerim", "/Customer/Address"),
                    ("Aktif Kargolarım", "/Customer/Cargo?OnlyActive=true"),
                    ("Geçmiş Teslimatlar", "/Customer/Cargo?Status=Delivered")
                });
            }

            var current = $"{Request.Path}{Request.QueryString}";
            var items = links
                .Select(l => new MenuItemViewModel(l.Title, l.Url,
                    string.Equals(l.Url, current, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return View(items);
        }
    }
}
