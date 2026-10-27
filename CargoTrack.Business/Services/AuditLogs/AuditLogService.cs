using CargoTrack.Business.Helpers;
using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.DTO.DTOs.AuditLogDtos;
using CargoTrack.DTO.DTOs.Common;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace CargoTrack.Business.Services.AuditLogs
{
    public class AuditLogService(IAuditLogRepository _auditLogRepository,
                                 UserManager<AppUser> _userManager) : IAuditLogService
    {
        public async Task LogAsync(Guid? userId, AuditActionType actionType, string entityName, Guid entityId,
                                   string? oldValue, string? newValue, string? description)
        {
            string? userName = null;
            if (userId.HasValue)
            {
                var user = await _userManager.FindByIdAsync(userId.Value.ToString());
                userName = user?.UserName;
            }

            await _auditLogRepository.CreateAsync(new AuditLog
            {
                UserId = userId,
                UserName = userName,
                ActionType = actionType,
                EntityName = entityName,
                EntityId = entityId,
                OldValue = oldValue,
                NewValue = newValue,
                Description = description
            });
        }

        public async Task<PagedResultDto<ResultAuditLogDto>> GetPagedAsync(AuditLogFilterDto filter)
        {
            var query = _auditLogRepository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim();
                query = query.Where(x => (x.UserName != null && x.UserName.Contains(term)) ||
                                         (x.Description != null && x.Description.Contains(term)));
            }
            if (filter.ActionType.HasValue)
                query = query.Where(x => x.ActionType == filter.ActionType.Value);
            if (!string.IsNullOrWhiteSpace(filter.EntityName))
                query = query.Where(x => x.EntityName == filter.EntityName);
            if (filter.StartDate.HasValue)
                query = query.Where(x => x.CreatedDate >= filter.StartDate.Value.Date);
            if (filter.EndDate.HasValue)
            {
                var end = filter.EndDate.Value.Date.AddDays(1);
                query = query.Where(x => x.CreatedDate < end);
            }

            return await query.OrderByDescending(x => x.CreatedDate)
                              .ProjectToType<ResultAuditLogDto>()
                              .ToPagedResultAsync(filter.Page, filter.PageSize);
        }
    }
}