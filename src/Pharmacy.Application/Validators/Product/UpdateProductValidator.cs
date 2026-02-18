using FluentValidation;
using Pharmacy.Application.Commands.Product;

namespace Pharmacy.Application.Validators.Product;

/// <summary>
/// Validator for UpdateProductCommand
/// </summary>
public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Product.Oid)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.Product.DrugName)
            .NotEmpty().WithMessage("Drug name is required")
            .MaximumLength(500).WithMessage("Drug name cannot exceed 500 characters");

        RuleFor(x => x.Product.GTIN)
            .MaximumLength(14).WithMessage("GTIN cannot exceed 14 characters")
            .When(x => !string.IsNullOrEmpty(x.Product.GTIN));

        RuleFor(x => x.Product.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value")
            .When(x => x.Product.Price.HasValue);
    }
}
