using FluentValidation;
using Pharmacy.Application.Commands.Product;

namespace Pharmacy.Application.Validators.Product;

/// <summary>
/// Validator for CreateProductCommand
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Product.DrugName)
            .NotEmpty().WithMessage("Drug name is required")
            .MaximumLength(500).WithMessage("Drug name cannot exceed 500 characters");

        RuleFor(x => x.Product.GTIN)
            .MaximumLength(14).WithMessage("GTIN cannot exceed 14 characters")
            .When(x => !string.IsNullOrEmpty(x.Product.GTIN));

        RuleFor(x => x.Product.GenericName)
            .MaximumLength(500).WithMessage("Generic name cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Product.GenericName));

        RuleFor(x => x.Product.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value")
            .When(x => x.Product.Price.HasValue);

        RuleFor(x => x.Product.MinStockLevel)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum stock level must be a positive value")
            .When(x => x.Product.MinStockLevel.HasValue);

        RuleFor(x => x.Product.MaxStockLevel)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum stock level must be a positive value")
            .When(x => x.Product.MaxStockLevel.HasValue);

        RuleFor(x => x.Product)
            .Must(p => !p.MinStockLevel.HasValue || !p.MaxStockLevel.HasValue || p.MinStockLevel <= p.MaxStockLevel)
            .WithMessage("Minimum stock level cannot exceed maximum stock level");
    }
}
