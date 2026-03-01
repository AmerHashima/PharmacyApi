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
    public string? SerialNumber { get; set; }
    public decimal Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Notes { get; set; }
}
