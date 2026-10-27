using CargoTrack.DTO.DTOs.Cargos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Cargos
{
    public class DeliverCargoValidator : AbstractValidator<DeliverCargoDto>
    {
        public DeliverCargoValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Kargo bilgisi bulunamadı.");
            RuleFor(x => x.DeliveryCode).NotEmpty().WithMessage("Teslimat kodu boş bırakılamaz.")
                .Matches(@"^\d{6}$").WithMessage("Teslimat kodu 6 haneli bir sayı olmalıdır.");
            RuleFor(x => x.ReceivedByName).NotEmpty().WithMessage("Teslim alan kişinin adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Teslim alan kişinin adı en fazla 100 karakter olabilir.");
        }
    }
}
