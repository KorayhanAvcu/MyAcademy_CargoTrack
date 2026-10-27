using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.DTO.DTOs.Common;

namespace CargoTrack.Business.Services.Cargos
{
    public interface ICargoService
    {
        // Oluşturma ve operasyon
        Task<string> CreateAsync(CreateCargoDto dto, Guid userId);
        Task ChangeStatusAsync(ChangeCargoStatusDto dto, Guid userId, Guid? employeeId);
        Task DeliverAsync(DeliverCargoDto dto, Guid userId, Guid employeeId);
        Task ReportFailureAsync(ReportDeliveryFailureDto dto, Guid userId, Guid employeeId);

        // Okuma
        Task<CargoTrackingDto?> GetByTrackCodeAsync(string trackCode);
        Task<CargoDetailDto> GetDetailAsync(Guid cargoId, Guid userId, bool isAdmin, Guid? managerBranchId);

        // Rol bazlı listeler (sayfalı)
        Task<PagedResultDto<ResultCargoDto>> GetPagedAsync(CargoFilterDto filter);                    // Admin
        Task<PagedResultDto<ResultCargoDto>> GetByBranchAsync(Guid branchId, CargoFilterDto filter);  // Manager
        Task<PagedResultDto<ResultCargoDto>> GetByUserAsync(Guid userId, CargoFilterDto filter);      // User
    }
}