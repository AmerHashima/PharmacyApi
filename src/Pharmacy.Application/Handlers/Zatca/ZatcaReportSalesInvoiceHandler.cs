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
        // Items with no VatType on the product are zero-rated medicines (VATEX-SA-35)
        var lines = invoice.Items.Select((item, index) =>
        {
            var unitPrice = item.UnitPrice ?? 0m;
            var qty = item.Quantity;
            var discount = item.DiscountAmount ?? 0m;

            var isStandard = item.Product?.VatTypeId != null;
            var vatPercent = isStandard ? 15m : 0m;
            var taxCategory = isStandard ? TaxCategoryIdType.S : TaxCategoryIdType.Z;

            return new InvoiceLineData(
                id: (index + 1).ToString(),
                invoicedQuantity: qty,
                name: item.Product?.DrugName ?? item.Product?.DrugNameAr ?? "Product",
                priceAmount: unitPrice,
                vatPercent: vatPercent,
                taxCategoryId: taxCategory,
                discount: discount);
        }).ToList();

        // ── Tax subtotals — one entry per tax category ────────────────────
        var subtotals = new List<InvoiceTaxSubtotal>();

        var standardLines = lines.Where(l => l.TaxCategoryId == TaxCategoryIdType.S).ToList();
        if (standardLines.Count > 0)
        {
            var stdTaxable = standardLines.Sum(l => l.LineExtensionAmount);
            var stdTax = Math.Round(stdTaxable * 0.15m, 2);
            subtotals.Add(new InvoiceTaxSubtotal(
                taxAmount: stdTax,
                taxableAmount: stdTaxable,
                vatPercent: 15m,
                taxCategoryId: TaxCategoryIdType.S));
        }

        var zeroLines = lines.Where(l => l.TaxCategoryId == TaxCategoryIdType.Z).ToList();
        if (zeroLines.Count > 0)
        {
            var zeroTaxable = zeroLines.Sum(l => l.LineExtensionAmount);
            subtotals.Add(new InvoiceTaxSubtotal(
                taxAmount: 0m,
                taxableAmount: zeroTaxable,
                vatPercent: 0m,
                taxCategoryId: TaxCategoryIdType.Z,
                taxExemptionReasonCode: ZatcaTaxExemptionReasonCode.VATEX_SA_35,
                taxExemptionReason: TaxExemptionReason.MedicinesAndMedicalEquipment));
        }

        // ── Tax total (header) ────────────────────────────────────────────
        var totalTaxable = lines.Sum(l => l.LineExtensionAmount);
        var totalDiscount = invoice.DiscountAmount ?? lines.Sum(l => l.Discount);
        var totalTax = invoice.TaxAmount ?? subtotals.Sum(s => s.TaxAmount);

        var taxTotal = new InvoiceTaxTotal(
            taxAmount: totalTax,
            taxableAmount: totalTaxable,
            vatPercent: totalTaxable > 0 ? Math.Round(totalTax / totalTaxable * 100m, 2) : 0m,
            totalDiscount: totalDiscount)
        {
            TaxSubtotal = subtotals
        };

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
