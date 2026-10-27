using CargoTrack.DTO.DTOs.BranchDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Branches
{
    public class CreateBranchValidator : AbstractValidator<CreateBranchDto>
    {
        public CreateBranchValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Şube adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Şube adı en fazla 100 karakter olabilir.");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir seçilmelidir.");
        }
    }
}
