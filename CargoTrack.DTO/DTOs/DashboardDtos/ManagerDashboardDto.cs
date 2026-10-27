namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class ManagerDashboardDto
    {
        public int TodayIncomingCount { get; set; }
        public int TodayOutgoingCount { get; set; }
        public int OutForDeliveryCount { get; set; }
        public int DeliveredTodayCount { get; set; }
        public int DelayedCount { get; set; }
        public int ProblematicCount { get; set; }
        public List<RecentMovementDto> RecentMovements { get; set; } = new();
    }
}