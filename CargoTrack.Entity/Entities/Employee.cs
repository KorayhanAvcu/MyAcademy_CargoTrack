using CargoTrack.Entity.Entities.Common;

namespace CargoTrack.Entity.Entities
{
    public class Employee : BaseEntity
    {
        public string Position { get; set; }   
        public Guid AppUserId { get; set; }
        public Guid BranchId { get; set; }

        //Navigation Properties
        public virtual AppUser AppUser { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual IList<CargoMovement> Movements { get; set; }
    }
}
