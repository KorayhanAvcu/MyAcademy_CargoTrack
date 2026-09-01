using CargoTrack.DTO.DTOs.BranchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.CityDtos
{
    public class ResultCityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<BranchDto> Branches { get; set; }
    }
}
