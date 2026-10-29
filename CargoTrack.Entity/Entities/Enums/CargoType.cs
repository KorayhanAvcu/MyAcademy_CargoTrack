using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum CargoType
    {
        [Display(Name = "Standart")] Standard = 1,
        [Display(Name = "Acil")] Urgent = 2,
        [Display(Name = "Kırılacak")] Fragile = 3,
        [Display(Name = "Ağır")] Heavy = 4
    }
}