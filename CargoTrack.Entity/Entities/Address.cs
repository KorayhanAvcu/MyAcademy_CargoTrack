using CargoTrack.Entity.Entities.Common;

namespace CargoTrack.Entity.Entities
{
    public class Address : BaseEntity
    {
        public string Title { get; set; }
        public Guid CityId { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public Guid UserId { get; set; }

        public virtual City City { get; set; }
        public virtual AppUser User { get; set; }
    }
}
