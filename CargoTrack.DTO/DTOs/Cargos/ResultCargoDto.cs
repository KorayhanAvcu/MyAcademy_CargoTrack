using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.Cargos
{
    public class ResultCargoDto
    {
        public Guid Id { get; set; }
        public string TrackCode { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public decimal Price { get; set; }

        public string OriginBranchName { get; set; }
        public string OriginBranchCityName { get; set; }
        public string DestinationBranchName { get; set; }
        public string DestinationBranchCityName { get; set; }
    }
}
