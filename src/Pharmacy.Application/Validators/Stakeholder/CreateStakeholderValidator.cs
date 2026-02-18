using FluentValidation;
using Pharmacy.Application.Commands.Stakeholder;

namespace Pharmacy.Application.Validators.Stakeholder;

/// <summary>
/// Validator for CreateStakeholderCommand
/// </summary>
public class CreateStakeholderValidator : AbstractValidator<CreateStakeholderCommand>
{
    public CreateStakeholderValidator()
    {
        RuleFor(x => x.Stakeholder.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Stakeholder.GLN)
            .MaximumLength(20).WithMessage("GLN cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Stakeholder.GLN));

        RuleFor(x => x.Stakeholder.City)
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Stakeholder.City));

        RuleFor(x => x.Stakeholder.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Stakeholder.Email));

        RuleFor(x => x.Stakeholder.Phone)
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Stakeholder.Phone));
    }
}
