namespace Pharmacy.Application.DTOs.Product;

/// <summary>
/// Response DTO containing parsed barcode data and associated product
/// </summary>
public class BarcodeProductResponseDto
{
    /// <summary>
    /// Parsed barcode information
    /// </summary>
    public BarcodeParseResponseDto BarcodeData { get; set; } = new();

    /// <summary>
    /// Product details found by GTIN (null if not found)
    /// </summary>
    public ProductDto? Product { get; set; }

    /// <summary>
    /// Indicates if product was found in database
    /// </summary>
    public bool ProductFound { get; set; }

    /// <summary>
    /// Message about product lookup result
    /// </summary>
    public string? ProductMessage { get; set; }
}
