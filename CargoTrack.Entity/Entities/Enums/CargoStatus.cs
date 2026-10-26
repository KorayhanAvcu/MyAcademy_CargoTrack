namespace CargoTrack.Entity.Entities.Enums
{
    public enum CargoStatus
    {
        Created = 1,             // Oluşturuldu
        AtOriginBranch = 2,      // Gönderici Şubesinde
        InTransferCenter = 3,    // Transfer Merkezinde
        AtDestinationBranch = 4, // Varış Şubesinde
        OutForDelivery = 5,      // Dağıtıma Çıktı
        Delivered = 6,           // Teslim Edildi
        DeliveryFailed = 7,      // Teslim Edilemedi
        ReturnInProgress = 8,    // İade Sürecinde
        ReturnedToSender = 9     // Göndericiye İade Edildi
    }
}
