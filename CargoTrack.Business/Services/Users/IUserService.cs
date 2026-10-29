using CargoTrack.DTO.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace CargoTrack.Business.Services.Users
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<List<ResultUserDto>> GetCustomersAsync();
        Task<List<ResultUserDto>> GetByRoleAsync(string roleName);
        Task<List<ResultUserDto>> GetAllWithRolesAsync();
        Task<(string FullName, List<RoleAssignDto> Roles)> GetRoleAssignAsync(Guid userId);
        Task AssignRolesAsync(List<RoleAssignDto> model);
    }
}