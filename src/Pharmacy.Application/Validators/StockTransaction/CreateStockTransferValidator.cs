using FluentValidation;
using Pharmacy.Application.Commands.StockTransaction;

namespace Pharmacy.Application.Validators.StockTransaction;

/// <summary>
/// Validator for CreateStockTransferCommand
/// </summary>
public class CreateStockTransferValidator : AbstractValidator<CreateStockTransferCommand>
{
    public CreateStockTransferValidator()
    {
        RuleFor(x => x.Transfer.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.Transfer.FromBranchId)
            .NotEmpty().WithMessage("From Branch ID is required");

        RuleFor(x => x.Transfer.ToBranchId)
            .NotEmpty().WithMessage("To Branch ID is required")
            .NotEqual(x => x.Transfer.FromBranchId).WithMessage("Cannot transfer to the same branch");

        RuleFor(x => x.Transfer.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.Transfer.BatchNumber)
            .MaximumLength(50).WithMessage("Batch number cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Transfer.BatchNumber));
    }
}
