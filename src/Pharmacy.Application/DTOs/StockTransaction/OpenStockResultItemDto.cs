namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// Resolved result for a single Open Stock line.
/// Contains the identified product details alongside the supplied stock data.
/// </summary>
public class OpenStockResultItemDto
{
    public int LineNumber { get; set; }

    /// <summary>GTIN or Barcode that was submitted</summary>
    public string GtinOrBarcode { get; set; } = string.Empty;

    // ── Resolved product fields ──────────────────────────────────────────────
    public Guid ProductId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string DrugNameAr { get; set; } = string.Empty;
    public string? GTIN { get; set; }
    public string? Barcode { get; set; }
    public string? GenericName { get; set; }
    public string? ProductTypeName { get; set; }

    // ── Stock data supplied by the user ──────────────────────────────────────
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
