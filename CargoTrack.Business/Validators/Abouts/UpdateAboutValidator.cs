using CargoTrack.DTO.DTOs.AboutDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Abouts
{
    public class UpdateAboutValidator : AbstractValidator<UpdateAboutDto>
    {
        public UpdateAboutValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş bırakılamaz.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz.");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel Url boş bırakılamaz.");
        }
    }
}
