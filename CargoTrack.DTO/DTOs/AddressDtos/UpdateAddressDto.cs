namespace CargoTrack.DTO.DTOs.AddressDtos
{
    public class UpdateAddressDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CityId { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
    }
}
