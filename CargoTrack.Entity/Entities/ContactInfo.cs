

using CargoTrack.Entity.Entities.Common;

namespace CargoTrack.Entity.Entities
{
    public class ContactInfo : BaseEntity
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
