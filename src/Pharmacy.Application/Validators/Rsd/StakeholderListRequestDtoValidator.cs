using FluentValidation;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Validators.Rsd;

public class StakeholderListRequestDtoValidator : AbstractValidator<StakeholderListRequestDto>
{
    public StakeholderListRequestDtoValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch ID is required");
        RuleFor(x => x.StakeholderType).InclusiveBetween(1, 5)
            .WithMessage("StakeholderType must be between 1 and 5 (1=Pharmacy, 2=Supplier, 3=Distributor, 4=Manufacturer, 5=Wholesaler)");
    }
}
