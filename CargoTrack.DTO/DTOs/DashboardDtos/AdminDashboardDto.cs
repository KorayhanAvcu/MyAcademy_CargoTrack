namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class AdminDashboardDto
    {
        public int TotalCargoCount { get; set; }
        public int ActiveCargoCount { get; set; }
        public int DelayedCargoCount { get; set; }
        public double DeliveryRate { get; set; }
        public double OnTimeDeliveryRate { get; set; }
        public int ActiveBranchCount { get; set; }
        public int EmployeeCount { get; set; }
        public List<DailyCountDto> Last7Days { get; set; } = new();
        public List<StatusCountDto> StatusDistribution { get; set; } = new();
        public List<BranchCountDto> TopBranches { get; set; } = new();
    }
}
