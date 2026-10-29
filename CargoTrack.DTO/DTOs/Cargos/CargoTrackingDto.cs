using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    public class CargoTrackingDto
    {
        public string TrackCode { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public double Weight { get; set; }
        public CargoType CargoType { get; set; }

        public string SenderMaskedName { get; set; }
        public string ReceiverMaskedName { get; set; }

        public string OriginBranchName { get; set; }
        public string OriginBranchCityName { get; set; }
        public string DestinationBranchName { get; set; }
        public string DestinationBranchCityName { get; set; }

        public List<CargoMovementDto> Movements { get; set; }
    }
}
