using CargoTrack.Business.Helpers;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.DTO.DTOs.DashboardDtos;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.Dashboards
{
    public class DashboardService(ICargoRepository _cargoRepository,
                                  ICargoMovementRepository _cargoMovementRepository,
                                  IBranchRepository _branchRepository,
                                  IEmployeeRepository _employeeRepository,
                                  ICargoService _cargoService) : IDashboardService
    {
        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var cargos = _cargoRepository.GetQueryable();

            var total = await cargos.CountAsync();
            var delivered = await cargos.CountAsync(c => c.CargoStatus == CargoStatus.Delivered);
            var onTime = await cargos.CountAsync(c => c.CargoStatus == CargoStatus.Delivered &&
                                                      c.DeliveredDate!.Value.Date <= c.EstimatedArrivalDate.Date);

            var weekStart = DateTime.Today.AddDays(-6);
            var daily = await cargos.Where(c => c.ShipmentDate >= weekStart)
                                    .GroupBy(c => c.ShipmentDate.Date)
                                    .Select(g => new { Date = g.Key, Count = g.Count() })
                                    .ToListAsync();

            return new AdminDashboardDto
            {
                TotalCargoCount = total,
                ActiveCargoCount = await cargos.WhereActive().CountAsync(),
                DelayedCargoCount = await cargos.WhereDelayed().CountAsync(),
                DeliveryRate = Percent(delivered, total),
                OnTimeDeliveryRate = Percent(onTime, delivered),
                ActiveBranchCount = await _branchRepository.CountAsync(),
                EmployeeCount = await _employeeRepository.CountAsync(),

                Last7Days = Enumerable.Range(0, 7)
                    .Select(i => weekStart.AddDays(i))
                    .Select(day => new DailyCountDto
                    {
                        Date = day,
                        Count = daily.FirstOrDefault(x => x.Date == day)?.Count ?? 0
                    })
                    .ToList(),

                StatusDistribution = await cargos
                    .GroupBy(c => c.CargoStatus)
                    .Select(g => new StatusCountDto { Status = g.Key, Count = g.Count() })
                    .ToListAsync(),

                TopBranches = await _branchRepository.GetQueryable()
                    .Select(b => new BranchCountDto
                    {
                        BranchName = b.Name,
                        Count = b.OriginCargos.Count + b.DestinationCargos.Count
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToListAsync()
            };
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardAsync(Guid branchId)
        {
            var today = DateTime.Today;
            var branchCargos = _cargoRepository.GetQueryable()
                .Where(c => c.OriginBranchId == branchId || c.DestinationBranchId == branchId);
            var movements = _cargoMovementRepository.GetQueryable();

            return new ManagerDashboardDto
            {
                TodayIncomingCount = await movements.CountAsync(m => m.BranchId == branchId &&
                                                                     m.NewStatus == CargoStatus.AtDestinationBranch &&
                                                                     m.MovementDate >= today),
                TodayOutgoingCount = await movements.CountAsync(m => m.Cargo.OriginBranchId == branchId &&
                                                                     m.OldStatus == CargoStatus.AtOriginBranch &&
                                                                     m.MovementDate >= today),
                OutForDeliveryCount = await branchCargos.CountAsync(c => c.DestinationBranchId == branchId &&
                                                                         c.CargoStatus == CargoStatus.OutForDelivery),
                DeliveredTodayCount = await branchCargos.CountAsync(c => c.DestinationBranchId == branchId &&
                                                                         c.DeliveredDate >= today),
                DelayedCount = await branchCargos.WhereDelayed().CountAsync(),
                ProblematicCount = await branchCargos.CountAsync(c => c.CargoStatus == CargoStatus.DeliveryFailed ||
                                                                      c.CargoStatus == CargoStatus.ReturnInProgress),
                RecentMovements = await movements
                    .Where(m => m.Cargo.OriginBranchId == branchId || m.Cargo.DestinationBranchId == branchId)
                    .OrderByDescending(m => m.MovementDate)
                    .Take(10)
                    .ProjectToType<RecentMovementDto>()
                    .ToListAsync()
            };
        }

        public async Task<UserDashboardDto> GetUserDashboardAsync(Guid userId)
        {
            var myCargos = _cargoRepository.GetQueryable()
                .Where(c => c.SenderId == userId || c.ReceiverId == userId);

            var lastCargoId = await myCargos.OrderByDescending(c => c.ShipmentDate)
                                            .Select(c => (Guid?)c.Id)
                                            .FirstOrDefaultAsync();

            return new UserDashboardDto
            {
                ActiveCargoCount = await myCargos.WhereActive().CountAsync(),
                IncomingActiveCount = await myCargos.WhereActive().CountAsync(c => c.ReceiverId == userId),
                OutgoingActiveCount = await myCargos.WhereActive().CountAsync(c => c.SenderId == userId),
                DeliveredCount = await myCargos.CountAsync(c => c.CargoStatus == CargoStatus.Delivered),
                ActiveCargos = await myCargos.WhereActive()
                    .OrderBy(c => c.EstimatedArrivalDate)
                    .Take(5)
                    .ProjectToType<ResultCargoDto>()
                    .ToListAsync(),

                LastCargo = lastCargoId is null
                    ? null
                    : await _cargoService.GetDetailAsync(lastCargoId.Value, userId, isAdmin: false, managerBranchId: null)
            };
        }

        private static double Percent(int part, int total)
        {
            return total == 0 ? 0 : Math.Round(part * 100.0 / total, 1);
        }
    }
}
