using CargoTrack.Business.Exceptions;
using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.CargoPrices
{
    public class CargoPriceService(ICargoPriceRepository _cargoPriceRepository) : ICargoPriceService
    {
        public async Task<List<ResultCargoPriceDto>> GetAllAsync()
        {
            return await _cargoPriceRepository.GetQueryable()
                .OrderBy(x => x.DeliveryType).ThenBy(x => x.DistanceType).ThenBy(x => x.MinDesi)
                .ProjectToType<ResultCargoPriceDto>()
                .ToListAsync();
        }

        public async Task<UpdateCargoPriceDto> GetByIdAsync(Guid id)
        {
            var price = await GetOrThrowAsync(id);
            return price.Adapt<UpdateCargoPriceDto>();
        }

        public async Task CreateAsync(CreateCargoPriceDto dto)
        {
            await EnsureNoOverlapAsync(dto.DeliveryType, dto.DistanceType, dto.MinDesi, dto.MaxDesi, null);
            await _cargoPriceRepository.CreateAsync(dto.Adapt<CargoPrice>());
        }

        public async Task UpdateAsync(UpdateCargoPriceDto dto)
        {
            await EnsureNoOverlapAsync(dto.DeliveryType, dto.DistanceType, dto.MinDesi, dto.MaxDesi, dto.Id);

            var price = await GetOrThrowAsync(dto.Id);
            dto.Adapt(price);
            await _cargoPriceRepository.UpdateAsync(price);
        }

        public async Task DeleteAsync(Guid id)
        {
            var price = await GetOrThrowAsync(id);
            await _cargoPriceRepository.DeleteAsync(price);
        }

        // Aynı tip + mesafe için desi aralıkları çakışmamalı, yoksa fiyat belirsiz olur
        private async Task EnsureNoOverlapAsync(DeliveryType deliveryType, DistanceType distanceType,
                                                double minDesi, double maxDesi, Guid? excludeId)
        {
            var hasOverlap = await _cargoPriceRepository.AnyAsync(p =>
                p.DeliveryType == deliveryType &&
                p.DistanceType == distanceType &&
                p.MinDesi <= maxDesi && minDesi <= p.MaxDesi &&
                (excludeId == null || p.Id != excludeId));

            if (hasOverlap)
                throw new BusinessException("Bu desi aralığı mevcut bir fiyat kuralıyla çakışıyor.");
        }

        private async Task<CargoPrice> GetOrThrowAsync(Guid id)
        {
            return await _cargoPriceRepository.GetByIdAsync(id)
                   ?? throw new BusinessException("Fiyat kuralı bulunamadı.");
        }
    }
}