using FluentValidation;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;

namespace Pharmacy.Application.Validators.BranchIntegrationSetting;

public class CreateBranchIntegrationSettingDtoValidator : AbstractValidator<CreateBranchIntegrationSettingDto>
{
    public CreateBranchIntegrationSettingDtoValidator()
    {
        RuleFor(x => x.IntegrationProviderId)
            .NotEmpty().WithMessage("Integration provider is required");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch is required");

        RuleFor(x => x.IntegrationKey)
            .MaximumLength(255).WithMessage("Integration key cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.IntegrationKey));

        RuleFor(x => x.IntegrationValue)
            .MaximumLength(255).WithMessage("Integration value cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.IntegrationValue));

        RuleFor(x => x.Status)
            .InclusiveBetween(0, 2).WithMessage("Status must be 0 (Inactive), 1 (Active), or 2 (Testing)");
    }
}