using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    // Manager hareket girişi. BranchId veya TransferCenterId'den yalnızca biri dolu olmalı
    public class ChangeCargoStatusDto
    {
        public Guid CargoId { get; set; }
        public CargoStatus NewStatus { get; set; }
        public string? Description { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? TransferCenterId { get; set; }
    }
}
