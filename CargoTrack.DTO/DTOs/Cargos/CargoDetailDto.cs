using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    public class CargoDetailDto
    {
        public Guid Id { get; set; }
        public string TrackCode { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public CargoType CargoType { get; set; }
        public DeliveryType DeliveryType { get; set; }

        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        public double Weight { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double Desi { get; set; }
        public decimal Price { get; set; }

        public int FailedAttemptCount { get; set; }
        public string? DeliveryCode { get; set; }

        public Guid SenderId { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public Guid ReceiverId { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }

        public Guid OriginBranchId { get; set; }
        public string OriginBranchName { get; set; }
        public string OriginBranchCityName { get; set; }
        public Guid DestinationBranchId { get; set; }
        public string DestinationBranchName { get; set; }
        public string DestinationBranchCityName { get; set; }
        public string? DeliveryAddress { get; set; }

        public List<CargoMovementDto> Movements { get; set; }
    }
}
