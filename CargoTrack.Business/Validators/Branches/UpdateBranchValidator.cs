using CargoTrack.DTO.DTOs.BranchDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Branches
{
    public class UpdateBranchValidator : AbstractValidator<UpdateBranchDto>
    {
        public UpdateBranchValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Şube bilgisi bulunamadı.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Şube adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Şube adı en fazla 100 karakter olabilir.");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir seçilmelidir.");
        }
    }
}
