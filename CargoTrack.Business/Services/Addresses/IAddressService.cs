using CargoTrack.DTO.DTOs.AddressDtos;

namespace CargoTrack.Business.Services.Addresses
{
    public interface IAddressService
    {
        Task<List<ResultAddressDto>> GetByUserAsync(Guid userId);
        Task<UpdateAddressDto> GetForUpdateAsync(Guid id, Guid userId);
        Task CreateAsync(CreateAddressDto dto, Guid userId);
        Task UpdateAsync(UpdateAddressDto dto, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
