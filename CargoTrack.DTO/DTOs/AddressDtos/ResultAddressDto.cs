namespace CargoTrack.DTO.DTOs.AddressDtos
{
    public class ResultAddressDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }

        public string FullText => $"{Title} – {FullAddress}, {District} / {CityName}";
    }
}
