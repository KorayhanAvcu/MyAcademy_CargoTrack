using CargoTrack.Entity.Entities.Common;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Entity.Entities
{
    public class DeliveryException : BaseEntity
    {
        public Guid CargoId { get; set; }
        public Guid EmployeeId { get; set; }
        public DeliveryExceptionReason Reason { get; set; }
        public string? Description { get; set; }
        public int AttemptNumber { get; set; }   // 1, 2, 3...
        public DateTime AttemptDate { get; set; }

        //Navigation Properties
        public virtual Cargo Cargo { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
