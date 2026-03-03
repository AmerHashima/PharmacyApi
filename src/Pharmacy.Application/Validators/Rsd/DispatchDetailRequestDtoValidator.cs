using FluentValidation;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Validators.Rsd;

public class DispatchDetailRequestDtoValidator : AbstractValidator<DispatchDetailRequestDto>
{
    public DispatchDetailRequestDtoValidator()
    {
        RuleFor(x => x.DispatchNotificationId)
            .NotEmpty().WithMessage("Dispatch notification ID is required");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");
    }
}