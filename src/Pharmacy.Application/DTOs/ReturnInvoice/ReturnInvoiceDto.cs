namespace Pharmacy.Application.DTOs.ReturnInvoice;

/// <summary>
/// DTO for reading Return Invoice data
/// </summary>
public class ReturnInvoiceDto
{
    public Guid Oid { get; set; }
    public string ReturnNumber { get; set; } = string.Empty;
    public Guid OriginalInvoiceId { get; set; }
    public string? OriginalInvoiceNumber { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public decimal? SubTotal { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? RefundAmount { get; set; }
    public DateTime? ReturnDate { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public Guid? InvoiceStatusId { get; set; }
    public string? InvoiceStatusName { get; set; }
    public Guid? CashierId { get; set; }
    public string? CashierName { get; set; }
    public Guid? ReturnReasonId { get; set; }
    public string? ReturnReasonName { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<ReturnInvoiceItemDto> Items { get; set; } = new();
    public int ItemCount => Items.Count;
}
