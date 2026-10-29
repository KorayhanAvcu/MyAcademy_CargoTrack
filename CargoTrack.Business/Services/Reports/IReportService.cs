using CargoTrack.DTO.DTOs.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Reports
{
    public interface IReportService
    {
        Task<ReportDto> GetReportAsync();
    }
}
