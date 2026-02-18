using FluentValidation;
using Pharmacy.Application.Commands.StockTransaction;

namespace Pharmacy.Application.Validators.StockTransaction;

/// <summary>
/// Validator for CreateStockInCommand
/// </summary>
public class CreateStockInValidator : AbstractValidator<CreateStockInCommand>
{
    public CreateStockInValidator()
    {
        RuleFor(x => x.StockIn.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.StockIn.ToBranchId)
            .NotEmpty().WithMessage("Branch ID is required");

        RuleFor(x => x.StockIn.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.StockIn.UnitCost)
            .GreaterThanOrEqualTo(0).WithMessage("Unit cost must be a positive value")
            .When(x => x.StockIn.UnitCost.HasValue);

        RuleFor(x => x.StockIn.ExpiryDate)
            .GreaterThan(DateTime.Today).WithMessage("Expiry date must be in the future")
            .When(x => x.StockIn.ExpiryDate.HasValue);

        RuleFor(x => x.StockIn.BatchNumber)
            .MaximumLength(50).WithMessage("Batch number cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.StockIn.BatchNumber));
    }
}
