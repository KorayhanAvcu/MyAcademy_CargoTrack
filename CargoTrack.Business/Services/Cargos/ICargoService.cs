using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.DTO.DTOs.Common;

namespace CargoTrack.Business.Services.Cargos
{
    public interface ICargoService
    {
        Task<string> CreateAsync(CreateCargoDto dto, Guid userId);
        Task ChangeStatusAsync(ChangeCargoStatusDto dto, Guid userId, Guid? employeeId);
        Task DeliverAsync(DeliverCargoDto dto, Guid userId, Guid employeeId);
        Task ReportFailureAsync(ReportDeliveryFailureDto dto, Guid userId, Guid employeeId);

        Task<CargoTrackingDto?> GetByTrackCodeAsync(string trackCode);
        Task<CargoDetailDto> GetDetailAsync(Guid cargoId, Guid userId, bool isAdmin, Guid? managerBranchId);

        Task<PagedResultDto<ResultCargoDto>> GetPagedAsync(CargoFilterDto filter);
        Task<PagedResultDto<ResultCargoDto>> GetByBranchAsync(Guid branchId, CargoFilterDto filter);
        Task<PagedResultDto<ResultCargoDto>> GetByUserAsync(Guid userId, CargoFilterDto filter);
    }
}
