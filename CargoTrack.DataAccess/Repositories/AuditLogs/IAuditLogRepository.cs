using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;

namespace CargoTrack.DataAccess.Repositories.AuditLogs
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
    }
}