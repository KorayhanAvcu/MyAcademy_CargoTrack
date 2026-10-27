using CargoTrack.DTO.DTOs.TransferCenterDtos;

namespace CargoTrack.Business.Services.TransferCenters
{
    public interface ITransferCenterService
    {
        Task<List<ResultTransferCenterDto>> GetAllAsync();
        Task<UpdateTransferCenterDto> GetByIdAsync(Guid id);
        Task CreateAsync(CreateTransferCenterDto dto);
        Task UpdateAsync(UpdateTransferCenterDto dto);
        Task DeleteAsync(Guid id);
    }
}