using FluentValidation;
using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Application.Validators.Zatca;

public class ZatcaOnboardRequestDtoValidator : AbstractValidator<ZatcaOnboardRequestDto>
{
    public ZatcaOnboardRequestDtoValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch ID is required");
        RuleFor(x => x.OTP).NotEmpty().WithMessage("OTP is required");
        RuleFor(x => x.NameEn).NotEmpty().WithMessage("Company name (English) is required");
        RuleFor(x => x.UnitNameEn).NotEmpty().WithMessage("Unit name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(x => x.InvoicePortalType).Must(x => x is "1" or "2")
            .WithMessage("InvoicePortalType must be '1' (Production) or '2' (Simulation)");
    }
}
