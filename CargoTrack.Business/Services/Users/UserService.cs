using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace CargoTrack.Business.Services.Users
{
    public class UserService(UserManager<AppUser> _userManager) : IUserService
    {
        // WebUI'daki Roles.User ile aynı değer (Business, WebUI'ı referans alamaz)
        private const string CustomerRole = "User";

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            var user = dto.Adapt<AppUser>();

            var result = await _userManager.CreateAsync(user, dto.Password!);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, CustomerRole);

            return result;
        }

        public async Task<List<ResultUserDto>> GetCustomersAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(CustomerRole);
            return users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                        .Adapt<List<ResultUserDto>>();
        }
    }
}