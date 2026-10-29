namespace CargoTrack.DTO.DTOs.Cargos
{
    public class DeliverCargoDto
    {
        public Guid CargoId { get; set; }
        public string DeliveryCode { get; set; }
        public string ReceivedByName { get; set; }
    }
}
