using FluentValidation;
using Pharmacy.Application.DTOs.AppLookup;

namespace Pharmacy.Application.Validators.AppLookup;

public class CreateAppLookupMasterValidator : AbstractValidator<CreateAppLookupMasterDto>
{
    public CreateAppLookupMasterValidator()
    {
        RuleFor(x => x.LookupCode)
            .NotEmpty().WithMessage("Lookup code is required")
            .MaximumLength(50).WithMessage("Lookup code cannot exceed 50 characters")
            .Matches(@"^[A-Z_]+$").WithMessage("Lookup code must contain only uppercase letters and underscores");

        RuleFor(x => x.LookupNameAr)
            .NotEmpty().WithMessage("Arabic lookup name is required")
            .MaximumLength(100).WithMessage("Arabic lookup name cannot exceed 100 characters");

        RuleFor(x => x.LookupNameEn)
            .NotEmpty().WithMessage("English lookup name is required")
            .MaximumLength(100).WithMessage("English lookup name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description cannot exceed 250 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}