using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class EmployeePerformanceDto
    {
        public string FullName { get; set; }
        public string BranchName { get; set; }
        public int DeliveredCount { get; set; }
        public int FailedCount { get; set; }
        public int TodayOperationCount { get; set; }
    }
}
