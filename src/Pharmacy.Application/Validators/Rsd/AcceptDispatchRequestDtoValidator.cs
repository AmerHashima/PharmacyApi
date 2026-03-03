using FluentValidation;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Validators.Rsd;

public class AcceptDispatchRequestDtoValidator : AbstractValidator<AcceptDispatchRequestDto>
{
    public AcceptDispatchRequestDtoValidator()
    {
        RuleFor(x => x.DispatchNotificationId)
            .NotEmpty().WithMessage("Dispatch notification ID is required");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");
    }
}