namespace Pharmacy.Application.DTOs.SalesInvoice;

/// <summary>
/// DTO for reading Sales Invoice data
/// </summary>
public class SalesInvoiceDto
{
    public Guid Oid { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public decimal? SubTotal { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? ChangeAmount { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public Guid? InvoiceStatusId { get; set; }
    public string? InvoiceStatusName { get; set; }
    public Guid? CashierId { get; set; }
    public string? CashierName { get; set; }
    public string? PrescriptionNumber { get; set; }
    public string? DoctorName { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<SalesInvoiceItemDto> Items { get; set; } = new();
    public int ItemCount => Items.Count;
}
