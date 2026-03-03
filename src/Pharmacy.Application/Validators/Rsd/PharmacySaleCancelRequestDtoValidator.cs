using FluentValidation;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Validators.Rsd;

public class PharmacySaleCancelRequestDtoValidator : AbstractValidator<PharmacySaleCancelRequestDto>
{
    public PharmacySaleCancelRequestDtoValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch ID is required");
        RuleFor(x => x.Products).NotEmpty().WithMessage("At least one product is required");

        RuleForEach(x => x.Products).ChildRules(p =>
        {
            p.RuleFor(x => x.GTIN).NotEmpty().WithMessage("GTIN is required");
            p.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            p.RuleFor(x => x.BatchNumber).NotEmpty().WithMessage("Batch number is required");
            p.RuleFor(x => x.ExpiryDate).NotEmpty().WithMessage("Expiry date is required")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Expiry date must be yyyy-MM-dd");
        });
    }
}