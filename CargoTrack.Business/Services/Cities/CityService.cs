using CargoTrack.Business.Exceptions;
using CargoTrack.DataAccess.Repositories.Cities;
using CargoTrack.DTO.DTOs.CityDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.Cities
{
    public class CityService(ICityRepository _cityRepository) : ICityService
    {
        public async Task<List<ResultCityDto>> GetAllAsync()
        {
            return await _cityRepository.GetQueryable()
                .OrderBy(c => c.Name)
                .ProjectToType<ResultCityDto>()
                .ToListAsync();
        }

        public async Task<UpdateCityDto> GetByIdAsync(Guid id)
        {
            var city = await GetOrThrowAsync(id);
            return city.Adapt<UpdateCityDto>();
        }

        public async Task CreateAsync(CreateCityDto dto)
        {
            dto.Name = dto.Name.Trim();
            if (await _cityRepository.AnyAsync(c => c.Name == dto.Name))
                throw new BusinessException("Bu isimde bir şehir zaten var.");

            await _cityRepository.CreateAsync(dto.Adapt<City>());
        }

        public async Task UpdateAsync(UpdateCityDto dto)
        {
            dto.Name = dto.Name.Trim();
            if (await _cityRepository.AnyAsync(c => c.Name == dto.Name && c.Id != dto.Id))
                throw new BusinessException("Bu isimde bir şehir zaten var.");

            var city = await GetOrThrowAsync(dto.Id);
            dto.Adapt(city);
            await _cityRepository.UpdateAsync(city);
        }

        public async Task DeleteAsync(Guid id)
        {
            var city = await GetOrThrowAsync(id);
            await _cityRepository.DeleteAsync(city);
        }

        private async Task<City> GetOrThrowAsync(Guid id)
        {
            return await _cityRepository.GetByIdAsync(id)
                   ?? throw new BusinessException("Şehir bulunamadı.");
        }
    }
}
