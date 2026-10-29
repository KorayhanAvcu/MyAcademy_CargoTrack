namespace CargoTrack.DTO.DTOs.AddressDtos
{
    public class CreateAddressDto
    {
        public string Title { get; set; }
        public Guid CityId { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
    }
}
