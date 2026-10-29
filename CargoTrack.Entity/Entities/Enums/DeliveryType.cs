using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum DeliveryType
    {
        [Display(Name = "Standart Teslimat")] Standard = 1,
        [Display(Name = "Hızlı Teslimat")] Express = 2
    }
}