using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    // Teslim edilemedi bildirimi. Deneme numarasını servis hesaplar (FailedAttemptCount + 1)
    public class ReportDeliveryFailureDto
    {
        public Guid CargoId { get; set; }
        public DeliveryExceptionReason Reason { get; set; }
        public string? Description { get; set; }
    }
}
