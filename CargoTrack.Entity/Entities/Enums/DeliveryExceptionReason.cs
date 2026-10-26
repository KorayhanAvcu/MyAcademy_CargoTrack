namespace CargoTrack.Entity.Entities.Enums
{
    public enum DeliveryExceptionReason
    {
        WrongAddress = 1,       // Adres hatalı
        ReceiverNotFound = 2,   // Alıcı yok
        Damaged = 3,            // Hasarlı
        Rejected = 4            // Reddedildi
    }
}
