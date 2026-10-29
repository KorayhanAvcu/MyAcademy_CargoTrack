using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    public class ReportDeliveryFailureDto
    {
        public Guid CargoId { get; set; }
        public DeliveryExceptionReason Reason { get; set; }
        public string? Description { get; set; }
    }
}
