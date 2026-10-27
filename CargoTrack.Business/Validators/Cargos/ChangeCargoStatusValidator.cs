using CargoTrack.DTO.DTOs.Cargos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Cargos
{
    public class ChangeCargoStatusValidator : AbstractValidator<ChangeCargoStatusDto>
    {
        public ChangeCargoStatusValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Kargo bilgisi bulunamadı.");
            RuleFor(x => x.NewStatus).IsInEnum().WithMessage("Geçerli bir durum seçin.");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
            RuleFor(x => x)
                .Must(x => x.BranchId.HasValue ^ x.TransferCenterId.HasValue)
                .WithMessage("Şube veya transfer merkezinden yalnızca biri seçilmelidir.");
        }
    }
}
