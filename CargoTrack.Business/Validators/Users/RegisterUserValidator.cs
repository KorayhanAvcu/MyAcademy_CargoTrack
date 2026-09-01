using CargoTrack.DTO.DTOs.UserDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Validators.Users
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad bilgisi boş bırakılamaz.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad bilgisi boş bırakılamaz.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı bilgisi boş bırakılamaz.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email bilgisi boş bırakılamaz.").EmailAddress().WithMessage("Geçerli bir email adresi girin.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre bilgisi boş bırakılamaz.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Şifre tekrarı bilgisi boş bırakılamaz.").Matches(x=>x.Password).WithMessage("Şifreler birbiriyle uyumlu değil");
        }
    }
}
