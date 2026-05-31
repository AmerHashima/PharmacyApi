namespace Pharmacy.Application.DTOs.PurchaseInvoice;

public class PurchaseInvoiceDto
{
    public Guid Oid { get; set; }
    public string PurchaseInvoiceNumber { get; set; } = string.Empty;
    public string? SupplierInvoiceNumber { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Guid? StockTransactionId { get; set; }
    public Guid? JournalEntryId { get; set; }
    public Guid? FiscalYearId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount => TotalAmount - PaidAmount;
    public Guid? InvoiceStatusId { get; set; }
    public string? InvoiceStatusName { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<PurchaseInvoicePaymentDto> Payments { get; set; } = new();
}
