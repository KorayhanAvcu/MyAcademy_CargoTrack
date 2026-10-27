using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class StatusCountDto
    {
        public CargoStatus Status { get; set; }
        public int Count { get; set; }
    }
}