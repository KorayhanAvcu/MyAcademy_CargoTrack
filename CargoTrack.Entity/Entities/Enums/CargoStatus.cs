namespace CargoTrack.Entity.Entities.Enums
{
    public enum CargoStatus
    {
        Received = 1,
        InTransferCenter = 2,
        DispatchedFromTransferCenter = 3,
        ArrivedAtDeliveryBranch = 4,
        OutForDelivery = 5,
        Delivered = 6,
    }
}
