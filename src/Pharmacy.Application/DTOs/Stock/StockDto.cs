namespace Pharmacy.Application.DTOs.Stock;

/// <summary>
/// DTO for reading Stock data
/// </summary>
public class StockDto
{
    public Guid Oid { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductGTIN { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public decimal? Quantity { get; set; }
    public decimal? ReservedQuantity { get; set; }
    public decimal AvailableQuantity => (Quantity ?? 0) - (ReservedQuantity ?? 0);
    public DateTime? LastStockCountDate { get; set; }
    public decimal? AverageCost { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
