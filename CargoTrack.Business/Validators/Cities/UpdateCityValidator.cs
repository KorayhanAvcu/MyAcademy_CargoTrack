using CargoTrack.DTO.DTOs.CityDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Cities
{
    public class UpdateCityValidator : AbstractValidator<UpdateCityDto>
    {
        public UpdateCityValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Şehir bilgisi bulunamadı.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Şehir adı boş bırakılamaz.")
                .MaximumLength(50).WithMessage("Şehir adı en fazla 50 karakter olabilir.");
        }
    }
}
