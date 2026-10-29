using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum DistanceType
    {
        [Display(Name = "Şehir İçi")] SameCity = 1,
        [Display(Name = "Şehirler Arası")] Intercity = 2
    }
}