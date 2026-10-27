namespace CargoTrack.DTO.DTOs.TransferCenterDtos
{
    public class UpdateTransferCenterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CityId { get; set; }
    }
}