using CargoTrack.Business.Exceptions;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DTO.DTOs.BranchDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.Branches
{
    public class BranchService(IBranchRepository _branchRepository) : IBranchService
    {
        public async Task<List<ResultBranchDto>> GetAllAsync()
        {
            return await _branchRepository.GetQueryable()
                .OrderBy(b => b.Name)
                .ProjectToType<ResultBranchDto>()
                .ToListAsync();
        }

        public async Task<UpdateBranchDto> GetByIdAsync(Guid id)
        {
            var branch = await GetOrThrowAsync(id);
            return branch.Adapt<UpdateBranchDto>();
        }

        public async Task CreateAsync(CreateBranchDto dto)
        {
            dto.Name = dto.Name.Trim();
            if (await _branchRepository.AnyAsync(b => b.Name == dto.Name))
                throw new BusinessException("Bu isimde bir şube zaten var.");

            await _branchRepository.CreateAsync(dto.Adapt<Branch>());
        }

        public async Task UpdateAsync(UpdateBranchDto dto)
        {
            dto.Name = dto.Name.Trim();
            if (await _branchRepository.AnyAsync(b => b.Name == dto.Name && b.Id != dto.Id))
                throw new BusinessException("Bu isimde bir şube zaten var.");

            var branch = await GetOrThrowAsync(dto.Id);
            dto.Adapt(branch);
            await _branchRepository.UpdateAsync(branch);
        }

        public async Task DeleteAsync(Guid id)
        {
            var branch = await GetOrThrowAsync(id);
            await _branchRepository.DeleteAsync(branch);
        }

        private async Task<Branch> GetOrThrowAsync(Guid id)
        {
            return await _branchRepository.GetByIdAsync(id)
                   ?? throw new BusinessException("Şube bulunamadı.");
        }
    }
}
