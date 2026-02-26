using FluentValidation;
using Pharmacy.Application.DTOs.IntegrationProvider;

namespace Pharmacy.Application.Validators.IntegrationProvider;

public class UpdateIntegrationProviderDtoValidator : AbstractValidator<UpdateIntegrationProviderDto>
{
    public UpdateIntegrationProviderDtoValidator()
    {
        RuleFor(x => x.Oid)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Status)
            .InclusiveBetween(0, 2).WithMessage("Status must be 0 (Inactive), 1 (Active), or 2 (Suspended)");
    }
}