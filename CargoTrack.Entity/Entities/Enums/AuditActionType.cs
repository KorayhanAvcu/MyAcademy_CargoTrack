using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum AuditActionType
    {
        [Display(Name = "Ekleme")] Create = 1,
        [Display(Name = "Güncelleme")] Update = 2,
        [Display(Name = "Silme")] Delete = 3,
        [Display(Name = "Durum Değişikliği")] StatusChange = 4
    }
}