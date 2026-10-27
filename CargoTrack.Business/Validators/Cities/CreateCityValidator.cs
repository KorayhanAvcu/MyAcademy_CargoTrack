using CargoTrack.DTO.DTOs.CityDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Cities
{
    public class CreateCityValidator : AbstractValidator<CreateCityDto>
    {
        public CreateCityValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Şehir adı boş bırakılamaz.")
                .MaximumLength(50).WithMessage("Şehir adı en fazla 50 karakter olabilir.");
        }
    }
}
