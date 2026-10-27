using CargoTrack.DTO.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace CargoTrack.Business.Services.Users
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<List<ResultUserDto>> GetCustomersAsync();   // Kargo formundaki gönderici/alıcı dropdown'ı
    }
}