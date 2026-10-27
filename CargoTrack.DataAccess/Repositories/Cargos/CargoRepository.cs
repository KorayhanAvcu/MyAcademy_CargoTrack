using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.DataAccess.Repositories.Cargos
{
    public class CargoRepository : GenericRepository<Cargo>, ICargoRepository
    {
        public CargoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cargo?> GetByTrackCodeWithMovementsAsync(string trackCode)
        {
            return await _context.Cargos
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.OriginBranch).ThenInclude(b => b.City)
                .Include(x => x.DestinationBranch).ThenInclude(b => b.City)
                .Include(x => x.Movements).ThenInclude(m => m.Branch)
                .Include(x => x.Movements).ThenInclude(m => m.TransferCenter)
                .FirstOrDefaultAsync(x => x.TrackCode == trackCode);
        }
    }
}