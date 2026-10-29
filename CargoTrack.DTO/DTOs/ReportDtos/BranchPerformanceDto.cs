using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class BranchPerformanceDto
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public int SentCount { get; set; }
        public int IncomingCount { get; set; }
        public int DeliveredCount { get; set; }
        public int DelayedCount { get; set; }
        public int TodayOperationCount { get; set; }
        public double SuccessRate => IncomingCount == 0 ? 0 : Math.Round(DeliveredCount * 100.0 / IncomingCount, 1);
    }
}
