namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for reading StockTransactionDetail data
/// </summary>
public class StockTransactionDetailDto
{
    public Guid Oid { get; set; }
    public Guid StockTransactionId { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductGTIN { get; set; }
    public decimal Quantity { get; set; }
    public string? Gtin { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? SerialNumber { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? TotalCost { get; set; }
    public int LineNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
