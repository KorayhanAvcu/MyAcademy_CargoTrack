using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    public class CargoFilterDto
    {
        public string? Search { get; set; }              // Takip numarası
        public CargoStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool OnlyActive { get; set; }
        public bool OnlyDelayed { get; set; }
        public CargoDirection? Direction { get; set; }   // Sadece User listesinde kullanılır
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}