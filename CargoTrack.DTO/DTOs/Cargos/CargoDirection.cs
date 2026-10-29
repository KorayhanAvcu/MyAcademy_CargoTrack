using System.ComponentModel.DataAnnotations;

namespace CargoTrack.DTO.DTOs.Cargos
{
    public enum CargoDirection
    {
        [Display(Name = "Gelen")] Incoming = 1,
        [Display(Name = "Giden")] Outgoing = 2
    }
}
