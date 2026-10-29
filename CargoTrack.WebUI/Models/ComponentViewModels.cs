using CargoTrack.DTO.DTOs.Cargos;

namespace CargoTrack.WebUI.Models
{
    public record MenuItemViewModel(string Title, string Url, bool IsActive);

    public record AlertViewModel(string? Error, string? Success);

    public record StatusBadgeViewModel(string Text, string CssClass, string Icon);

    public record CargoRowViewModel(ResultCargoDto Cargo, bool IsDelayed);

    public record PageLinkViewModel(int Number, string Url, bool IsActive);

    public class PagerViewModel
    {
        public int TotalCount { get; set; }
        public string? PreviousUrl { get; set; }
        public string? NextUrl { get; set; }
        public List<PageLinkViewModel> Pages { get; set; } = new();
    }
}
