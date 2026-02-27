using FluentValidation;
using Pharmacy.Application.DTOs.Product;

namespace Pharmacy.Application.Validators.Product;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
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

        RuleFor(x => x.StrengthValue)
            .MaximumLength(50).WithMessage("Strength value cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.StrengthValue));

        RuleFor(x => x.StrengthUnit)
            .MaximumLength(50).WithMessage("Strength unit cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.StrengthUnit));

        RuleFor(x => x.PackageType)
            .MaximumLength(100).WithMessage("Package type cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.PackageType));

        RuleFor(x => x.PackageSize)
            .MaximumLength(50).WithMessage("Package size cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.PackageSize));

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0")
            .When(x => x.Price.HasValue);

        RuleFor(x => x.RegistrationNumber)
            .MaximumLength(50).WithMessage("Registration number cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.RegistrationNumber));

        RuleFor(x => x.Volume)
            .GreaterThan(0).WithMessage("Volume must be greater than 0")
            .When(x => x.Volume.HasValue);

        RuleFor(x => x.UnitOfVolume)
            .MaximumLength(50).WithMessage("Unit of volume cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.UnitOfVolume));

        RuleFor(x => x.Manufacturer)
            .MaximumLength(200).WithMessage("Manufacturer cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Manufacturer));

        RuleFor(x => x.CountryOfOrigin)
            .MaximumLength(100).WithMessage("Country of origin cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.CountryOfOrigin));

        //RuleFor(x => x.MinStockLevel)
        //    .GreaterThanOrEqualTo(0).WithMessage("Minimum stock level must be greater than or equal to 0")
        //    .When(x => x.MinStockLevel.HasValue);

        //RuleFor(x => x.MaxStockLevel)
        //    .GreaterThan(x => x.MinStockLevel).WithMessage("Maximum stock level must be greater than minimum stock level")
        //    .When(x => x.MaxStockLevel.HasValue && x.MinStockLevel.HasValue);
    }
}