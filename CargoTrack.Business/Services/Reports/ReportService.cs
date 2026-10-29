using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.ReportDtos;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.Reports
{
    public class ReportService(ICargoRepository _cargoRepository,
                               IBranchRepository _branchRepository,
                               IEmployeeRepository _employeeRepository,
                               ICargoMovementRepository _cargoMovementRepository) : IReportService
    {
        public async Task<ReportDto> GetReportAsync()
        {
            var today = DateTime.Today;
            var cargos = _cargoRepository.GetQueryable();

            var branches = await _branchRepository.GetQueryable()
                .Select(b => new BranchPerformanceDto
                {
                    BranchId = b.Id,
                    BranchName = b.Name,
                    SentCount = b.OriginCargos.Count,
                    IncomingCount = b.DestinationCargos.Count,
                    DeliveredCount = b.DestinationCargos.Count(c => c.CargoStatus == CargoStatus.Delivered),
                    DelayedCount = b.DestinationCargos.Count(c => c.CargoStatus != CargoStatus.Delivered &&
                                                                  c.CargoStatus != CargoStatus.ReturnedToSender &&
                                                                  c.EstimatedArrivalDate < today)
                })
                .ToListAsync();

            var todayOps = await _cargoMovementRepository.GetQueryable()
                .Where(m => m.MovementDate >= today && m.BranchId != null)
                .GroupBy(m => m.BranchId!.Value)
                .Select(g => new { BranchId = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var b in branches)
                b.TodayOperationCount = todayOps.FirstOrDefault(x => x.BranchId == b.BranchId)?.Count ?? 0;

            var employees = await _employeeRepository.GetQueryable()
                .Select(e => new EmployeePerformanceDto
                {
                    FullName = e.AppUser.FirstName + " " + e.AppUser.LastName,
                    BranchName = e.Branch.Name,
                    DeliveredCount = e.Movements.Count(m => m.NewStatus == CargoStatus.Delivered),
                    FailedCount = e.Movements.Count(m => m.NewStatus == CargoStatus.DeliveryFailed),
                    TodayOperationCount = e.Movements.Count(m => m.MovementDate >= today)
                })
                .OrderByDescending(e => e.DeliveredCount)
                .ToListAsync();

            var total = await cargos.CountAsync();
            var returned = await cargos.CountAsync(c => c.CargoStatus == CargoStatus.ReturnInProgress ||
                                                        c.CargoStatus == CargoStatus.ReturnedToSender);

            var delivered = await cargos
                .Where(c => c.CargoStatus == CargoStatus.Delivered && c.DeliveredDate != null)
                .Select(c => new { c.ShipmentDate, DeliveredDate = c.DeliveredDate!.Value, c.EstimatedArrivalDate })
                .ToListAsync();

            var onTime = delivered.Count(d => d.DeliveredDate.Date <= d.EstimatedArrivalDate.Date);

            return new ReportDto
            {
                AverageDeliveryHours = delivered.Count == 0
                    ? 0
                    : Math.Round(delivered.Average(d => (d.DeliveredDate - d.ShipmentDate).TotalHours), 1),
                OnTimeRate = Percent(onTime, delivered.Count),
                ReturnRate = Percent(returned, total),
                TopSenderBranch = branches.OrderByDescending(b => b.SentCount).FirstOrDefault()?.BranchName,
                TopDeliveryBranch = branches.OrderByDescending(b => b.DeliveredCount).FirstOrDefault()?.BranchName,
                Branches = branches.OrderByDescending(b => b.SentCount + b.IncomingCount).ToList(),
                MostDelayedBranches = branches.Where(b => b.DelayedCount > 0)
                                              .OrderByDescending(b => b.DelayedCount).Take(5).ToList(),
                Employees = employees
            };
        }

        private static double Percent(int part, int total) =>
            total == 0 ? 0 : Math.Round(part * 100.0 / total, 1);
    }
}
