using CargoTrack.Business.Exceptions;
using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.Users
{
    public class UserService(UserManager<AppUser> _userManager,
                             RoleManager<AppRole> _roleManager) : IUserService
    {
        private const string CustomerRole = "User";

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            var user = dto.Adapt<AppUser>();

            var result = await _userManager.CreateAsync(user, dto.Password!);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, CustomerRole);

            return result;
        }

        public Task<List<ResultUserDto>> GetCustomersAsync() => GetByRoleAsync(CustomerRole);

        public async Task<List<ResultUserDto>> GetByRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                        .Adapt<List<ResultUserDto>>();
        }

        public async Task<List<ResultUserDto>> GetAllWithRolesAsync()
        {
            var users = await _userManager.Users.OrderBy(u => u.FirstName).ToListAsync();
            var result = new List<ResultUserDto>();

            foreach (var user in users)
            {
                var dto = user.Adapt<ResultUserDto>();
                dto.Roles = await _userManager.GetRolesAsync(user);
                result.Add(dto);
            }
            return result;
        }

        public async Task<(string FullName, List<RoleAssignDto> Roles)> GetRoleAssignAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString())
                       ?? throw new BusinessException("Kullanıcı bulunamadı.");
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();

            var list = roles.Select(role => new RoleAssignDto
            {
                UserId = user.Id,
                RoleId = role.Id,
                RoleName = role.Name!,
                RoleExist = userRoles.Contains(role.Name!)
            }).ToList();

            return ($"{user.FirstName} {user.LastName}", list);
        }

        public async Task AssignRolesAsync(List<RoleAssignDto> model)
        {
            var userId = model.Select(x => x.UserId).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userId.ToString())
                       ?? throw new BusinessException("Kullanıcı bulunamadı.");
            var currentRoles = await _userManager.GetRolesAsync(user);

            foreach (var item in model)
            {
                var hasRole = currentRoles.Contains(item.RoleName);

                if (item.RoleExist && !hasRole)
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                else if (!item.RoleExist && hasRole)
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
            }
        }
    }
}
