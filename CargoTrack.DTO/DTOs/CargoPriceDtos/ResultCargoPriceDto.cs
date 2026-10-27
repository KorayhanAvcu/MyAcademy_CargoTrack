using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.CargoPriceDtos
{
    public class ResultCargoPriceDto
    {
        public Guid Id { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public DistanceType DistanceType { get; set; }
        public double MinDesi { get; set; }
        public double MaxDesi { get; set; }
        public decimal Price { get; set; }
    }
}