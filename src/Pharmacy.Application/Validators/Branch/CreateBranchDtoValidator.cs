using FluentValidation;
using Pharmacy.Application.DTOs.Branch;

namespace Pharmacy.Application.Validators.Branch;

public class CreateBranchDtoValidator : AbstractValidator<CreateBranchDto>
{
    public CreateBranchDtoValidator()
    {
        RuleFor(x => x.BranchCode)
            .NotEmpty().WithMessage("Branch code is required")
            .MaximumLength(50).WithMessage("Branch code cannot exceed 50 characters");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required")
            .MaximumLength(200).WithMessage("Branch name cannot exceed 200 characters");

        RuleFor(x => x.GLN)
            .MaximumLength(20).WithMessage("GLN cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.GLN));

        RuleFor(x => x.CRN)
            .MaximumLength(20).WithMessage("CRN cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.CRN));

        RuleFor(x => x.VatNumber)
            .MaximumLength(20).WithMessage("VAT number cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.VatNumber));

        RuleFor(x => x.IdentifyValue)
            .MaximumLength(20).WithMessage("Identify value cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.IdentifyValue));

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.District)
            .MaximumLength(100).WithMessage("District cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.StreetName)
            .MaximumLength(500).WithMessage("Street name cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.StreetName));

        RuleFor(x => x.BuildingNumber)
            .MaximumLength(500).WithMessage("Building number cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.BuildingNumber));

        RuleFor(x => x.PostalZone)
            .MaximumLength(500).WithMessage("Postal zone cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.PostalZone));

        RuleFor(x => x.RegistrationName)
            .MaximumLength(500).WithMessage("Registration name cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.RegistrationName));
    }
}