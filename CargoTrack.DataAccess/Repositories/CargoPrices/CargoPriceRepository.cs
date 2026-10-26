using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;

namespace CargoTrack.DataAccess.Repositories.CargoPrices
{
    public class CargoPriceRepository : GenericRepository<CargoPrice>, ICargoPriceRepository
    {
        public CargoPriceRepository(AppDbContext context) : base(context)
        {
        }
    }
}