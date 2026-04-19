namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// Represents a line item that could not be matched to any product.
/// </summary>
public class OpenStockNotFoundItemDto
{
    public int LineNumber { get; set; }

    /// <summary>The GTIN or Barcode value that was submitted but not matched.</summary>
    public string GtinOrBarcode { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Reason { get; set; } = "No product found for the supplied GTIN/Barcode";
}
