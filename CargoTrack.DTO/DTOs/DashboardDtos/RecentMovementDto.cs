using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class RecentMovementDto
    {
        public string CargoTrackCode { get; set; }
        public CargoStatus NewStatus { get; set; }
        public DateTime MovementDate { get; set; }
        public string? BranchName { get; set; }
        public string? TransferCenterName { get; set; }
    }
}
