using FluentValidation;
using Pharmacy.Application.DTOs.Role;

namespace Pharmacy.Application.Validators.Role;

public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required")
            .MaximumLength(50).WithMessage("Role name cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9\s_-]+$").WithMessage("Role name can only contain letters, numbers, spaces, underscores, and hyphens");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}