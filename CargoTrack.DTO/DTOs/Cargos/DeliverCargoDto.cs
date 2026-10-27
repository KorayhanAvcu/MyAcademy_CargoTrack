namespace CargoTrack.DTO.DTOs.Cargos
{
    // Teslimat kodu doğrulama
    public class DeliverCargoDto
    {
        public Guid CargoId { get; set; }
        public string DeliveryCode { get; set; }      // Alıcının söylediği 6 haneli kod
        public string ReceivedByName { get; set; }    // Teslim alan kişi
    }
}
