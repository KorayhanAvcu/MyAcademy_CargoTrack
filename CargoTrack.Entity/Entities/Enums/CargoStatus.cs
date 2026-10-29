using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum CargoStatus
    {
        [Display(Name = "Oluşturuldu")] Created = 1,
        [Display(Name = "Gönderici Şubesinde")] AtOriginBranch = 2,
        [Display(Name = "Transfer Merkezinde")] InTransferCenter = 3,
        [Display(Name = "Varış Şubesinde")] AtDestinationBranch = 4,
        [Display(Name = "Dağıtıma Çıktı")] OutForDelivery = 5,
        [Display(Name = "Teslim Edildi")] Delivered = 6,
        [Display(Name = "Teslim Edilemedi")] DeliveryFailed = 7,
        [Display(Name = "İade Sürecinde")] ReturnInProgress = 8,
        [Display(Name = "Göndericiye İade Edildi")] ReturnedToSender = 9
    }
}