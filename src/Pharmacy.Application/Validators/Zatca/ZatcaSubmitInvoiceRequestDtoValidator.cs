using FluentValidation;
using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Application.Validators.Zatca;

public class ZatcaSubmitInvoiceRequestDtoValidator : AbstractValidator<ZatcaSubmitInvoiceRequestDto>
{
    public ZatcaSubmitInvoiceRequestDtoValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch ID is required");
        RuleFor(x => x.InvoiceData).NotNull().WithMessage("Invoice data is required");
    }
}
