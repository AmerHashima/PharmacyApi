using FluentValidation;
using Pharmacy.Application.Commands.AppLookup;

namespace Pharmacy.Application.Validators.AppLookup;

/// <summary>
/// Validator for UpdateAppLookupDetailCommand
/// </summary>
public class UpdateAppLookupDetailValidator : AbstractValidator<UpdateAppLookupDetailCommand>
{
    public UpdateAppLookupDetailValidator()
    {
        RuleFor(x => x.LookupDetail.Oid)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.LookupDetail.MasterID)
            .NotEmpty().WithMessage("Lookup master ID is required");

        RuleFor(x => x.LookupDetail.ValueCode)
            .NotEmpty().WithMessage("Value code is required")
            .MaximumLength(50).WithMessage("Value code cannot exceed 50 characters");

        RuleFor(x => x.LookupDetail.ValueNameAr)
            .NotEmpty().WithMessage("Arabic value name is required")
            .MaximumLength(100).WithMessage("Arabic value name cannot exceed 100 characters");

        RuleFor(x => x.LookupDetail.ValueNameEn)
            .NotEmpty().WithMessage("English value name is required")
            .MaximumLength(100).WithMessage("English value name cannot exceed 100 characters");

        //RuleFor(x => x.LookupDetail.SortOrder)
        //    .GreaterThan(0).WithMessage("Sort order must be greater than 0");
    }
}
