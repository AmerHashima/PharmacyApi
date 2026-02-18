using FluentValidation;
using Pharmacy.Application.Commands.Branch;

namespace Pharmacy.Application.Validators.Branch;

/// <summary>
/// Validator for CreateBranchCommand
/// </summary>
public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.Branch.BranchCode)
            .NotEmpty().WithMessage("Branch code is required")
            .MaximumLength(50).WithMessage("Branch code cannot exceed 50 characters");

        RuleFor(x => x.Branch.BranchName)
            .NotEmpty().WithMessage("Branch name is required")
            .MaximumLength(200).WithMessage("Branch name cannot exceed 200 characters");

        RuleFor(x => x.Branch.GLN)
            .MaximumLength(20).WithMessage("GLN cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Branch.GLN));

        RuleFor(x => x.Branch.City)
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Branch.City));

        RuleFor(x => x.Branch.District)
            .MaximumLength(100).WithMessage("District cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Branch.District));

        RuleFor(x => x.Branch.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Branch.Address));
    }
}
