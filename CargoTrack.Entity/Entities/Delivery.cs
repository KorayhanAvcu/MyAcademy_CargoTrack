using CargoTrack.Entity.Entities.Common;

namespace CargoTrack.Entity.Entities
{
    public class Delivery : BaseEntity
    {
        public Guid CargoId { get; set; }
        public Guid EmployeeId { get; set; }
        public string ReceivedByName { get; set; }
        public DateTime DeliveredDate { get; set; }

        public virtual Cargo Cargo { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
