using CargoTrack.DTO.DTOs.CargoPriceDtos;

namespace CargoTrack.Business.Services.CargoPrices
{
    public interface ICargoPriceService
    {
        Task<List<ResultCargoPriceDto>> GetAllAsync();
        Task<UpdateCargoPriceDto> GetByIdAsync(Guid id);
        Task CreateAsync(CreateCargoPriceDto dto);
        Task UpdateAsync(UpdateCargoPriceDto dto);
        Task DeleteAsync(Guid id);
    }
}