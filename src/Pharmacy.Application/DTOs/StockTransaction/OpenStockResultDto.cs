namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// Resolved result returned after calling the Open Stock lookup endpoint.
/// Nothing is written to the database.
/// </summary>
public class OpenStockResultDto
{
    /// <summary>Total number of input lines submitted.</summary>
    public int TotalInputLines { get; set; }

    /// <summary>Number of lines successfully resolved to a product.</summary>
    public int TotalFound { get; set; }

    /// <summary>Number of lines that could not be matched to any product.</summary>
    public int TotalNotFound { get; set; }

    /// <summary>Sum of (Quantity × UnitCost) for all resolved lines.</summary>
    public decimal TotalValue { get; set; }

    /// <summary>Lines that were successfully resolved with full product details.</summary>
    public List<OpenStockResultItemDto> FoundItems { get; set; } = new();

    /// <summary>Lines whose GTIN or Barcode did not match any product.</summary>
    public List<OpenStockNotFoundItemDto> NotFoundItems { get; set; } = new();
}
