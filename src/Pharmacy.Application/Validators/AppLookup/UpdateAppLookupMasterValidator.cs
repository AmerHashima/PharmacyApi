using FluentValidation;
using Pharmacy.Application.Commands.AppLookup;

namespace Pharmacy.Application.Validators.AppLookup;

/// <summary>
/// Validator for UpdateAppLookupMasterCommand
/// </summary>
public class UpdateAppLookupMasterValidator : AbstractValidator<UpdateAppLookupMasterCommand>
{
    public UpdateAppLookupMasterValidator()
    {
        RuleFor(x => x.LookupMaster.Oid)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.LookupMaster.LookupCode)
            .NotEmpty().WithMessage("Lookup code is required")
            .MaximumLength(50).WithMessage("Lookup code cannot exceed 50 characters");
            // .Matches("^[A-Z_]+$").WithMessage("Lookup code must contain only uppercase letters and underscores");

        RuleFor(x => x.LookupMaster.LookupNameAr)
            .NotEmpty().WithMessage("Arabic lookup name is required")
            .MaximumLength(100).WithMessage("Arabic lookup name cannot exceed 100 characters");

        RuleFor(x => x.LookupMaster.LookupNameEn)
            .NotEmpty().WithMessage("English lookup name is required")
            .MaximumLength(100).WithMessage("English lookup name cannot exceed 100 characters");

        RuleFor(x => x.LookupMaster.Description)
            .MaximumLength(250).WithMessage("Description cannot exceed 250 characters")
            .When(x => !string.IsNullOrEmpty(x.LookupMaster.Description));
    }
}
