using CargoTrack.Business.Exceptions;
using CargoTrack.Business.Helpers;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.Common;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Services.Employees
{
    public class EmployeeService(IEmployeeRepository _employeeRepository,
                                 UserManager<AppUser> _userManager) : IEmployeeService
    {
        public async Task<PagedResultDto<ResultEmployeeDto>> GetPagedAsync(string? search, Guid? branchId, int page, int pageSize)
        {
            var query = _employeeRepository.GetQueryable();

            if (branchId.HasValue)
                query = query.Where(e => e.BranchId == branchId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(e => e.AppUser.FirstName.Contains(term) ||
                                         e.AppUser.LastName.Contains(term) ||
                                         e.Position.Contains(term));
            }

            return await query.OrderBy(e => e.AppUser.FirstName)
                              .ProjectToType<ResultEmployeeDto>()
                              .ToPagedResultAsync(page, pageSize);
        }

        public async Task<UpdateEmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await GetOrThrowAsync(id);
            return employee.Adapt<UpdateEmployeeDto>();
        }

        // Giriş yapan Manager'ın bağlı olduğu şubeyi bulmak için (yetki filtresi)
        public async Task<ResultEmployeeDto?> GetByAppUserIdAsync(Guid appUserId)
        {
            return await _employeeRepository.GetQueryable()
                .Where(e => e.AppUserId == appUserId)
                .ProjectToType<ResultEmployeeDto>()
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CreateEmployeeDto dto)
        {
            if (await _userManager.FindByIdAsync(dto.AppUserId.ToString()) is null)
                throw new BusinessException("Kullanıcı bulunamadı.");

            // AppUserId benzersiz: daha önce silinmiş kayıt varsa yenisini açmak yerine onu geri getir
            var existing = await _employeeRepository.GetQueryable()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.AppUserId == dto.AppUserId);

            if (existing is { IsDeleted: false })
                throw new BusinessException("Bu kullanıcı zaten personel olarak kayıtlı.");

            if (existing is not null)
            {
                existing.IsDeleted = false;
                existing.BranchId = dto.BranchId;
                existing.Position = dto.Position;
                await _employeeRepository.UpdateAsync(existing);
                return;
            }

            await _employeeRepository.CreateAsync(dto.Adapt<Employee>());
        }

        public async Task UpdateAsync(UpdateEmployeeDto dto)
        {
            var employee = await GetOrThrowAsync(dto.Id);
            dto.Adapt(employee);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await GetOrThrowAsync(id);
            await _employeeRepository.DeleteAsync(employee);   // Soft delete
        }

        private async Task<Employee> GetOrThrowAsync(Guid id)
        {
            return await _employeeRepository.GetByIdAsync(id)
                   ?? throw new BusinessException("Personel bulunamadı.");
        }
    }
}