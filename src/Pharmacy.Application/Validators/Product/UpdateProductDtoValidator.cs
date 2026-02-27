using FluentValidation;
using Pharmacy.Application.DTOs.Product;

namespace Pharmacy.Application.Validators.Product;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Oid)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.DrugName)
            .NotEmpty().WithMessage("Drug name is required")
            .MaximumLength(500).WithMessage("Drug name cannot exceed 500 characters");

        RuleFor(x => x.DrugNameAr)
            .MaximumLength(500).WithMessage("Arabic drug name cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.DrugNameAr));

        RuleFor(x => x.Barcode)
            .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Barcode));

        RuleFor(x => x.GTIN)
            .MaximumLength(14).WithMessage("GTIN cannot exceed 14 characters")
            .When(x => !string.IsNullOrEmpty(x.GTIN));

        RuleFor(x => x.GenericName)
            .MaximumLength(500).WithMessage("Generic name cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.GenericName));

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0")
            .When(x => x.Price.HasValue);

        //RuleFor(x => x.MinStockLevel)
        //    .GreaterThanOrEqualTo(0).WithMessage("Minimum stock level must be greater than or equal to 0")
        //    .When(x => x.MinStockLevel.HasValue);

        //RuleFor(x => x.MaxStockLevel)
        //    .GreaterThan(x => x.MinStockLevel).WithMessage("Maximum stock level must be greater than minimum stock level")
        //    .When(x => x.MaxStockLevel.HasValue && x.MinStockLevel.HasValue);
    }
}