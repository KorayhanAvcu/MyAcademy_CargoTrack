using CargoTrack.DTO.DTOs.Common;
using CargoTrack.DTO.DTOs.EmployeeDtos;

namespace CargoTrack.Business.Services.Employees
{
    public interface IEmployeeService
    {
        Task<PagedResultDto<ResultEmployeeDto>> GetPagedAsync(string? search, Guid? branchId, int page, int pageSize);
        Task<UpdateEmployeeDto> GetByIdAsync(Guid id);
        Task<ResultEmployeeDto?> GetByAppUserIdAsync(Guid appUserId);
        Task CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(UpdateEmployeeDto dto);
        Task DeleteAsync(Guid id);
    }
}