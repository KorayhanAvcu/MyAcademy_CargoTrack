namespace CargoTrack.DTO.DTOs.EmployeeDtos
{
    public class UpdateEmployeeDto
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public string Position { get; set; }
    }
}