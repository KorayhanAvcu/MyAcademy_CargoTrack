using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;

namespace CargoTrack.DataAccess.Repositories.TransferCenters
{
    public class TransferCenterRepository : GenericRepository<TransferCenter>, ITransferCenterRepository
    {
        public TransferCenterRepository(AppDbContext context) : base(context)
        {
        }
    }
}