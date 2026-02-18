using FluentValidation;
using Pharmacy.Application.Commands.SalesInvoice;

namespace Pharmacy.Application.Validators.SalesInvoice;

/// <summary>
/// Validator for CreateSalesInvoiceCommand
/// </summary>
public class CreateSalesInvoiceValidator : AbstractValidator<CreateSalesInvoiceCommand>
{
    public CreateSalesInvoiceValidator()
    {
        RuleFor(x => x.Invoice.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");

        RuleFor(x => x.Invoice.Items)
            .NotEmpty().WithMessage("At least one item is required")
            .Must(items => items.Count > 0).WithMessage("At least one item is required");

        RuleFor(x => x.Invoice.CustomerEmail)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Invoice.CustomerEmail));

        RuleFor(x => x.Invoice.DiscountPercent)
            .InclusiveBetween(0, 100).WithMessage("Discount percent must be between 0 and 100")
            .When(x => x.Invoice.DiscountPercent.HasValue);

        RuleForEach(x => x.Invoice.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            item.RuleFor(i => i.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price must be a positive value")
                .When(i => i.UnitPrice.HasValue);

            item.RuleFor(i => i.DiscountPercent)
                .InclusiveBetween(0, 100).WithMessage("Item discount percent must be between 0 and 100")
                .When(i => i.DiscountPercent.HasValue);
        });
    }
}
