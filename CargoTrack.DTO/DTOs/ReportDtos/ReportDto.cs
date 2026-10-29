using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class ReportDto
    {
        public double AverageDeliveryHours { get; set; }
        public double OnTimeRate { get; set; }
        public double ReturnRate { get; set; }
        public string? TopSenderBranch { get; set; }
        public string? TopDeliveryBranch { get; set; }
        public List<BranchPerformanceDto> Branches { get; set; } = new();
        public List<BranchPerformanceDto> MostDelayedBranches { get; set; } = new();
        public List<EmployeePerformanceDto> Employees { get; set; } = new();
    }
}
