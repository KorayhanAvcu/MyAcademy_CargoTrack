using CargoTrack.DTO.DTOs.Cargos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Cargos
{
    public class ReportDeliveryFailureValidator : AbstractValidator<ReportDeliveryFailureDto>
    {
        public ReportDeliveryFailureValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Kargo bilgisi bulunamadı.");
            RuleFor(x => x.Reason).IsInEnum().WithMessage("Geçerli bir sebep seçin.");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
