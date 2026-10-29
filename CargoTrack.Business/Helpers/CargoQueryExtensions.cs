using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Helpers
{
    public static class CargoQueryExtensions
    {
        public static IQueryable<Cargo> WhereActive(this IQueryable<Cargo> query)
        {
            return query.Where(c => c.CargoStatus != CargoStatus.Delivered &&
                                    c.CargoStatus != CargoStatus.ReturnedToSender);
        }

        public static IQueryable<Cargo> WhereDelayed(this IQueryable<Cargo> query)
        {
            var today = DateTime.Today;
            return query.WhereActive().Where(c => c.EstimatedArrivalDate < today);
        }
    }
}
