using CargoTrack.Business.Exceptions;
using CargoTrack.DataAccess.Repositories.Addresses;
using CargoTrack.DTO.DTOs.AddressDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.Addresses
{
    public class AddressService(IAddressRepository _addressRepository) : IAddressService
    {
        public async Task<List<ResultAddressDto>> GetByUserAsync(Guid userId)
        {
            return await _addressRepository.GetQueryable()
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.Title)
                .ProjectToType<ResultAddressDto>()
                .ToListAsync();
        }

        public async Task<UpdateAddressDto> GetForUpdateAsync(Guid id, Guid userId)
        {
            var address = await GetOwnedAsync(id, userId);
            return address.Adapt<UpdateAddressDto>();
        }

        public async Task CreateAsync(CreateAddressDto dto, Guid userId)
        {
            var address = dto.Adapt<Address>();
            address.UserId = userId;
            Normalize(address);
            await _addressRepository.CreateAsync(address);
        }

        public async Task UpdateAsync(UpdateAddressDto dto, Guid userId)
        {
            var address = await GetOwnedAsync(dto.Id, userId);
            dto.Adapt(address);
            address.UserId = userId;
            Normalize(address);
            await _addressRepository.UpdateAsync(address);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var address = await GetOwnedAsync(id, userId);
            await _addressRepository.DeleteAsync(address);
        }

        private static void Normalize(Address address)
        {
            address.Title = address.Title.Trim();
            address.District = address.District.Trim();
            address.FullAddress = address.FullAddress.Trim();
        }

        private async Task<Address> GetOwnedAsync(Guid id, Guid userId)
        {
            var address = await _addressRepository.GetByIdAsync(id)
                          ?? throw new BusinessException("Adres bulunamadı.");

            if (address.UserId != userId)
                throw new UnauthorizedAccessException("Bu adres size ait değil.");

            return address;
        }
    }
}
