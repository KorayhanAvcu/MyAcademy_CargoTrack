using CargoTrack.Entity.Entities.Common;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Entity.Entities
{
    public class CargoMovement : BaseEntity
    {
        public Guid CargoId { get; set; }
        public CargoStatus? OldStatus { get; set; }   // ilk kayıtta önceki durum yok
        public CargoStatus NewStatus { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Description { get; set; }

        public Guid? BranchId { get; set; }           // ya şubede
        public Guid? TransferCenterId { get; set; }   // ya transfer merkezinde
        public Guid? EmployeeId { get; set; }         // işlemi yapan personel

        //Navigation Properties
        public virtual Cargo Cargo { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual TransferCenter? TransferCenter { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
