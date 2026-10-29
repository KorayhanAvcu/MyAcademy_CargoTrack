using CargoTrack.Entity.Entities.Common;

namespace CargoTrack.Entity.Entities
{
    public class TransferCenter : BaseEntity
    {
        public string Name { get; set; }
        public Guid CityId { get; set; }

        public virtual City City { get; set; }
        public virtual IList<CargoMovement> Movements { get; set; }
    }
}
