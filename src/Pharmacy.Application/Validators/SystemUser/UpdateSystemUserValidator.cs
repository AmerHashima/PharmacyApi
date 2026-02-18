using FluentValidation;
using Pharmacy.Application.Commands.SystemUserSpace;

namespace Pharmacy.Application.Validators.SystemUser;

public class UpdateSystemUserValidator : AbstractValidator<UpdateSystemUserCommand>
{
    public UpdateSystemUserValidator()
    {
        RuleFor(x => x.SystemUser.Oid)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.SystemUser.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
            .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Username can only contain letters, numbers, dots, underscores, and hyphens.");

        RuleFor(x => x.SystemUser.Email)
            .EmailAddress().WithMessage("Invalid email address format.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SystemUser.Email));

        RuleFor(x => x.SystemUser.Mobile)
            .MaximumLength(20).WithMessage("Mobile must not exceed 20 characters.")
            .Matches(@"^[\+]?[0-9\-\s\(\)]+$").WithMessage("Invalid mobile number format.")
            .When(x => !string.IsNullOrEmpty(x.SystemUser.Mobile));

        RuleFor(x => x.SystemUser.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.SystemUser.MiddleName)
            .MaximumLength(50).WithMessage("Middle name must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.SystemUser.MiddleName));

        RuleFor(x => x.SystemUser.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        //RuleFor(x => x.SystemUser.GenderLookupId)
        //    .Must(g => g == null || g == 'M' || g == 'F').WithMessage("Gender must be 'M' or 'F'.")
        //    .When(x => x.SystemUser.Gender.HasValue);

    }
}