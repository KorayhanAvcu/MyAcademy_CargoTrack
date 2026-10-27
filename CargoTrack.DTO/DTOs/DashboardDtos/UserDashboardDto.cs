using CargoTrack.DTO.DTOs.Cargos;

namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class UserDashboardDto
    {
        public int ActiveCargoCount { get; set; }
        public int IncomingActiveCount { get; set; }
        public int OutgoingActiveCount { get; set; }
        public int DeliveredCount { get; set; }
        public List<ResultCargoDto> ActiveCargos { get; set; } = new();
        public CargoDetailDto? LastCargo { get; set; }   // Son kargo durumu + zaman çizelgesi + tahmini teslim
    }
}