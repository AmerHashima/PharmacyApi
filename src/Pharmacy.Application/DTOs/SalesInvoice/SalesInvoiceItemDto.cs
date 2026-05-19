namespace Pharmacy.Application.DTOs.SalesInvoice;

/// <summary>
/// DTO for reading Sales Invoice Item data
/// </summary>
public class SalesInvoiceItemDto
{
    public Guid Oid { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductGTIN { get; set; }
    public int LineNumber { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? RemainingQuantity { get; set; }
    public decimal? ReturnedQuantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxPercent { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? NetPrice { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? BatchNumber { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsFreeItem { get; set; }
    public Guid? OfferDetailId { get; set; }
    public string? OfferNameSnapshot { get; set; }
    public string? Notes { get; set; }
}
