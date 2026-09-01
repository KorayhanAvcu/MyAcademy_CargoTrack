using CargoTrack.DTO.DTOs.BranchDtos;

namespace CargoTrack.Business.Services.Branches
{
    public interface IBranchService
    {
        Task<List<ResultBranchDto>> GetAllAsync();
        Task<UpdateBranchDto> GetByIdAsync(Guid id);
        Task CreateAsync(CreateBranchDto createBranchDto);
        Task UpdateAsync(UpdateBranchDto updateBranchDto);
        Task DeleteAsync(Guid id);
    }
}
