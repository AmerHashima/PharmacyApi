using MediatR;
using Pharmacy.Application.Commands.Zatca;
using Pharmacy.Application.DTOs.Zatca;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using Zatca.Enums;
using Zatca.Models;
using ZatcaCustomer = Zatca.Models.Customer;

namespace Pharmacy.Application.Handlers.Zatca;

public class ZatcaReportSalesInvoiceHandler : IRequestHandler<ZatcaReportSalesInvoiceCommand, ZatcaSubmitInvoiceResponseDto>
{
    private readonly ISalesInvoiceRepository _invoiceRepository;
    private readonly IZatcaIntegrationService _zatcaService;

    public ZatcaReportSalesInvoiceHandler(
        ISalesInvoiceRepository invoiceRepository,
        IZatcaIntegrationService zatcaService)
    {
        _invoiceRepository = invoiceRepository;
        _zatcaService = zatcaService;
    }

    public async Task<ZatcaSubmitInvoiceResponseDto> Handle(ZatcaReportSalesInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetForZatcaAsync(request.InvoiceId, cancellationToken)
            ?? throw new InvalidOperationException($"Invoice '{request.InvoiceId}' not found.");

        var branch = invoice.Branch
            ?? throw new InvalidOperationException($"Invoice '{request.InvoiceId}' has no branch loaded.");

        // ── Supplier (Branch) ─────────────────────────────────────────────
        var supplier = new Supplier(
            schemeID: SellerSchemeIDType.CRN,
            schemaValue: branch.CRN ?? branch.VatNumber ?? string.Empty,
            streetName: branch.StreetName ?? branch.Address ?? string.Empty,
            buildingNumber: branch.BuildingNumber ?? string.Empty,
            cityName: branch.CityName ?? branch.City ?? string.Empty,
            companyID: branch.VatNumber ?? string.Empty,
            registrationName: branch.RegistrationName ?? branch.BranchName,
            postalZone: branch.PostalZone ?? string.Empty,
            plotIdentification: string.Empty,
            citySubdivisionName: branch.CitySubdivisionName ?? branch.District ?? string.Empty);

        // ── Customer ──────────────────────────────────────────────────────
        var customer = invoice.Customer is { } c
            ? new ZatcaCustomer
            {
                SchemeID = string.IsNullOrEmpty(c.VatNumber) ? CustomerSchemeIDType.NAT : CustomerSchemeIDType.TIN,
                SchemaValue = c.VatNumber ?? c.IdentityNumber ?? string.Empty,
                StreetName = c.AddressStreet ?? string.Empty,
                BuildingNumber = c.AddressBuildingNumber ?? string.Empty,
                PostalZone = c.AddressPostalCode ?? string.Empty,
                PlotIdentification = c.AddressAdditionalNumber ?? string.Empty,
                CityName = c.AddressCity ?? string.Empty,
                CompanyID = c.VatNumber ?? string.Empty,
                RegistrationName = c.NameEN,
                CitySubdivisionName = c.AddressDistrict ?? string.Empty
            }
            : new ZatcaCustomer { RegistrationName = "Cash Customer" };

        // ── Invoice lines ─────────────────────────────────────────────────
        var lines = invoice.Items.Select((item, index) =>
        {
            var unitPrice = item.UnitPrice ?? 0m;
            var qty = item.Quantity;
            var discount = item.DiscountAmount ?? 0m;
            var lineNet = qty * unitPrice - discount;
            var vatPercent = 15m;
            var taxAmount = lineNet * vatPercent / 100m;

            return new InvoiceLineData(
                id: (index + 1).ToString(),
                invoicedQuantity: qty,
                name: item.Product?.DrugName ?? item.Product?.DrugNameAr ?? "Product",
                priceAmount: unitPrice,
                vatPercent: vatPercent,
                taxCategoryId: TaxCategoryIdType.S,
                discount: discount);
        }).ToList();

        // ── Tax total ─────────────────────────────────────────────────────
        var subTotal = invoice.SubTotal ?? lines.Sum(l => l.LineExtensionAmount);
        var totalDiscount = invoice.DiscountAmount ?? 0m;
        var taxableAmount = subTotal - totalDiscount;
        var taxAmount = invoice.TaxAmount ?? taxableAmount * 0.15m;

        var taxTotal = new InvoiceTaxTotal(
            taxAmount: taxAmount,
            taxableAmount: taxableAmount,
            totalDiscount: totalDiscount,
            vatPercent: 15m);

        // ── InvoiceData ───────────────────────────────────────────────────
        var issueDate = invoice.InvoiceDate ?? DateTime.UtcNow;
        var invoiceData = new InvoiceData
        {
            ID = invoice.InvoiceNumber,
            UUid = invoice.Oid.ToString(),
            IssueDate = issueDate.ToString("yyyy-MM-dd"),
            IssueTime = issueDate.ToString("HH:mm:ss"),
            InvoiceTypeCodeValue = 388,  // Standard tax invoice
            Supplier = supplier,
            Customer = customer,
            TaxTotal = taxTotal,
            InvoiceLines = lines
        };

        var submitRequest = new ZatcaSubmitInvoiceRequestDto
        {
            BranchId = invoice.BranchId,
            InvoiceData = invoiceData
        };

        return await _zatcaService.ReportInvoiceAsync(submitRequest, cancellationToken);
    }
}
