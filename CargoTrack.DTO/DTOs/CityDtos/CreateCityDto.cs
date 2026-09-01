using CargoTrack.DTO.DTOs.BranchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.CityDtos
{
    public class CreateCityDto
    {
        public IList<BranchDto> Branches { get; set; }
    }
}
