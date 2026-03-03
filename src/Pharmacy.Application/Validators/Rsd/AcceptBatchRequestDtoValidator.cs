using FluentValidation;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Validators.Rsd;

public class AcceptBatchRequestDtoValidator : AbstractValidator<AcceptBatchRequestDto>
{
    public AcceptBatchRequestDtoValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("At least one product is required");

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(p => p.GTIN)
                .NotEmpty().WithMessage("GTIN is required");

            product.RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            product.RuleFor(p => p.BatchNumber)
                .NotEmpty().WithMessage("Batch number is required");

            product.RuleFor(p => p.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Expiry date must be in format yyyy-MM-dd");
        });
    }
}