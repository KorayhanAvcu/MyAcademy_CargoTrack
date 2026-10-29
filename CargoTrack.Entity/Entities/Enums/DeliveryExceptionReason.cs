using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum DeliveryExceptionReason
    {
        [Display(Name = "Adres Hatalı")] WrongAddress = 1,
        [Display(Name = "Alıcı Bulunamadı")] ReceiverNotFound = 2,
        [Display(Name = "Hasarlı")] Damaged = 3,
        [Display(Name = "Reddedildi")] Rejected = 4
    }
}