using CargoTrack.DTO.DTOs.TransferCenterDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.TransferCenters
{
    public class UpdateTransferCenterValidator : AbstractValidator<UpdateTransferCenterDto>
    {
        public UpdateTransferCenterValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Transfer merkezi bilgisi bulunamadı.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Transfer merkezi adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Transfer merkezi adı en fazla 100 karakter olabilir.");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir seçilmelidir.");
        }
    }
}
