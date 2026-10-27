using CargoTrack.DTO.DTOs.EmployeeDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Employees
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.AppUserId).NotEmpty().WithMessage("Kullanıcı seçilmelidir.");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Şube seçilmelidir.");
            RuleFor(x => x.Position).NotEmpty().WithMessage("Pozisyon boş bırakılamaz.")
                .MaximumLength(50).WithMessage("Pozisyon en fazla 50 karakter olabilir.");
        }
    }
}
