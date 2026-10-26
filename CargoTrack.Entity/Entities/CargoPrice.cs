using CargoTrack.Entity.Entities.Common;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Entity.Entities
{
    public class CargoPrice : BaseEntity
    {
        public DeliveryType DeliveryType { get; set; }
        public DistanceType DistanceType { get; set; }
        public double MinDesi { get; set; }
        public double MaxDesi { get; set; }
        public decimal Price { get; set; }
    }
}
