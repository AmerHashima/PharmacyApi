using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.PurchaseInvoice;

public class UpdatePurchaseInvoiceDto
{
    [MaxLength(100)]
    public string? SupplierInvoiceNumber { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public Guid? StockTransactionId { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? InvoiceStatusId { get; set; }

    public decimal? SubTotal { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
