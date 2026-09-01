using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.UserDtos
{
    public class RegisterUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        //[Compare(nameof(Password),ErrorMessage ="Şifreler birbiriyle uyumlu değil.")]
        public string? ConfirmPassword { get; set; }

        
    }
}
