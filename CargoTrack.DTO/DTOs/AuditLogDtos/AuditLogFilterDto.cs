using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.AuditLogDtos
{
    public class AuditLogFilterDto
    {
        public string? Search { get; set; }            // Kullanıcı adı veya açıklama
        public AuditActionType? ActionType { get; set; }
        public string? EntityName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}