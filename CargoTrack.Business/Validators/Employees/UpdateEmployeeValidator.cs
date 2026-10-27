using CargoTrack.DTO.DTOs.EmployeeDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Employees
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Personel bilgisi bulunamadı.");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Şube seçilmelidir.");
            RuleFor(x => x.Position).NotEmpty().WithMessage("Pozisyon boş bırakılamaz.")
                .MaximumLength(50).WithMessage("Pozisyon en fazla 50 karakter olabilir.");
        }
    }
}
