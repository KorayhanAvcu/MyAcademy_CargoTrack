using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;

namespace CargoTrack.DataAccess.Repositories.DeliveryExceptions
{
    public class DeliveryExceptionRepository : GenericRepository<DeliveryException>, IDeliveryExceptionRepository
    {
        public DeliveryExceptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}