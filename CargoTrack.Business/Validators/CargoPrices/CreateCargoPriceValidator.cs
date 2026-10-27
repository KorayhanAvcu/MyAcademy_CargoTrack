using CargoTrack.DTO.DTOs.CargoPriceDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.CargoPrices
{
    public class CreateCargoPriceValidator : AbstractValidator<CreateCargoPriceDto>
    {
        public CreateCargoPriceValidator()
        {
            RuleFor(x => x.DeliveryType).IsInEnum().WithMessage("Geçerli bir teslimat tipi seçin.");
            RuleFor(x => x.DistanceType).IsInEnum().WithMessage("Geçerli bir mesafe tipi seçin.");
            RuleFor(x => x.MinDesi).GreaterThanOrEqualTo(0).WithMessage("Minimum desi negatif olamaz.");
            RuleFor(x => x.MaxDesi).GreaterThan(x => x.MinDesi).WithMessage("Maksimum desi, minimum desiden büyük olmalıdır.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");
        }
    }
}
