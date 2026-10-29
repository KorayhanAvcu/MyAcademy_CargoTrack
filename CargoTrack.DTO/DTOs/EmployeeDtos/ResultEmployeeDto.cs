namespace CargoTrack.DTO.DTOs.EmployeeDtos
{
    public class ResultEmployeeDto
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public Guid BranchId { get; set; }
        public string Position { get; set; }

        public string AppUserFirstName { get; set; }
        public string AppUserLastName { get; set; }
        public string? AppUserEmail { get; set; }
        public string BranchName { get; set; }
        public string BranchCityName { get; set; }
    }
}
