using FluentValidation;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Validators.StockTransaction;

/// <summary>
/// Validator for creating stock transaction details
/// </summary>
public class CreateStockTransactionDetailValidator : AbstractValidator<CreateStockTransactionDetailDto>
{
    public CreateStockTransactionDetailValidator()
    {
        // StockTransactionId is required only when creating detail independently
        // When creating with master, it's set automatically
        

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.Gtin)
            .MaximumLength(50)
            .WithMessage("GTIN cannot exceed 50 characters");

        RuleFor(x => x.BatchNumber)
            .MaximumLength(50)
            .WithMessage("Batch number cannot exceed 50 characters");

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100)
            .WithMessage("Serial number cannot exceed 100 characters");

        RuleFor(x => x.UnitCost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.UnitCost.HasValue)
            .WithMessage("Unit cost must be a positive value");

        RuleFor(x => x.TotalCost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.TotalCost.HasValue)
            .WithMessage("Total cost must be a positive value");

        RuleFor(x => x.LineNumber)
            .GreaterThan(0)
            .WithMessage("Line number must be greater than 0");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters");
    }
}
