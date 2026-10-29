using CargoTrack.Business.Exceptions;
using CargoTrack.DataAccess.Repositories.TransferCenters;
using CargoTrack.DTO.DTOs.TransferCenterDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.TransferCenters
{
    public class TransferCenterService(ITransferCenterRepository _transferCenterRepository) : ITransferCenterService
    {
        public async Task<List<ResultTransferCenterDto>> GetAllAsync()
        {
            return await _transferCenterRepository.GetQueryable()
                .OrderBy(x => x.Name)
                .ProjectToType<ResultTransferCenterDto>()
                .ToListAsync();
        }

        public async Task<UpdateTransferCenterDto> GetByIdAsync(Guid id)
        {
            var transferCenter = await GetOrThrowAsync(id);
            return transferCenter.Adapt<UpdateTransferCenterDto>();
        }

        public async Task CreateAsync(CreateTransferCenterDto dto)
        {
            dto.Name = dto.Name.Trim();
            if (await _transferCenterRepository.AnyAsync(x => x.Name == dto.Name))
                throw new BusinessException("Bu isimde bir transfer merkezi zaten var.");

            await _transferCenterRepository.CreateAsync(dto.Adapt<TransferCenter>());
        }

        public async Task UpdateAsync(UpdateTransferCenterDto dto)
        {
            dto.Name = dto.Name.Trim();
            if (await _transferCenterRepository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id))
                throw new BusinessException("Bu isimde bir transfer merkezi zaten var.");

            var transferCenter = await GetOrThrowAsync(dto.Id);
            dto.Adapt(transferCenter);
            await _transferCenterRepository.UpdateAsync(transferCenter);
        }

        public async Task DeleteAsync(Guid id)
        {
            var transferCenter = await GetOrThrowAsync(id);
            await _transferCenterRepository.DeleteAsync(transferCenter);
        }

        private async Task<TransferCenter> GetOrThrowAsync(Guid id)
        {
            return await _transferCenterRepository.GetByIdAsync(id)
                   ?? throw new BusinessException("Transfer merkezi bulunamadı.");
        }
    }
}
