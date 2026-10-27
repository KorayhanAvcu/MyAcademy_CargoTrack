using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    // Herkese açık takipte de kullanıldığı için personel adı bilerek yok
    public class CargoMovementDto
    {
        public CargoStatus? OldStatus { get; set; }
        public CargoStatus NewStatus { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Description { get; set; }
        public string? LocationName { get; set; }   // Şube veya transfer merkezi adı
    }
}
