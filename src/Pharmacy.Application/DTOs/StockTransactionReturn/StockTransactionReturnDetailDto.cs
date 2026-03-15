namespace Pharmacy.Application.DTOs.StockTransactionReturn;

/// <summary>
/// DTO for reading StockTransactionReturnDetail data
/// </summary>
public class StockTransactionReturnDetailDto
{
    public Guid Oid { get; set; }
    public Guid StockTransactionReturnId { get; set; }
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
