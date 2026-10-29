using CargoTrack.DTO.DTOs.AddressDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Addresses
{
    public class UpdateAddressValidator : AbstractValidator<UpdateAddressDto>
    {
        public UpdateAddressValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Adres başlığı boş bırakılamaz (örn. Ev, İş).")
                .MaximumLength(50).WithMessage("Adres başlığı en fazla 50 karakter olabilir.");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir seçilmelidir.");
            RuleFor(x => x.District).NotEmpty().WithMessage("İlçe boş bırakılamaz.")
                .MaximumLength(100).WithMessage("İlçe en fazla 100 karakter olabilir.");
            RuleFor(x => x.FullAddress).NotEmpty().WithMessage("Açık adres boş bırakılamaz.")
                .MinimumLength(10).WithMessage("Açık adres en az 10 karakter olmalıdır.")
                .MaximumLength(500).WithMessage("Açık adres en fazla 500 karakter olabilir.");
        }
    }
}
