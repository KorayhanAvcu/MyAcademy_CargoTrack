using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.CargoPriceDtos
{
    public class CreateCargoPriceDto
    {
        public DeliveryType DeliveryType { get; set; }
        public DistanceType DistanceType { get; set; }
        public double MinDesi { get; set; }
        public double MaxDesi { get; set; }
        public decimal Price { get; set; }
    }
}