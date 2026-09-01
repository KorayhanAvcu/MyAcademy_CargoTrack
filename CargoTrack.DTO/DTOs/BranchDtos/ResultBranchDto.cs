using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.DTO.DTOs.CityDtos;

namespace CargoTrack.DTO.DTOs.BranchDtos
{
    public class ResultBranchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CityDto City { get; set; }
        public IList<CargoDto> OriginCargos { get; set; }
        public IList<CargoDto> DestinationCargos { get; set; }
    }
}
