using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.AuditLogDtos
{
    public class ResultAuditLogDto
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public AuditActionType ActionType { get; set; }
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}