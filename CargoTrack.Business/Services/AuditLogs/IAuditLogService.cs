using CargoTrack.DTO.DTOs.AuditLogDtos;
using CargoTrack.DTO.DTOs.Common;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Services.AuditLogs
{
    public interface IAuditLogService
    {
        Task LogAsync(Guid? userId, AuditActionType actionType, string entityName, Guid entityId,
                      string? oldValue, string? newValue, string? description);
        Task<PagedResultDto<ResultAuditLogDto>> GetPagedAsync(AuditLogFilterDto filter);
    }
}