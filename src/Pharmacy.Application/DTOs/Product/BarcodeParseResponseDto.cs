namespace Pharmacy.Application.DTOs.Product;

/// <summary>
/// Response DTO containing parsed barcode data
/// </summary>
public class BarcodeParseResponseDto
{
    /// <summary>
    /// Global Trade Item Number (14 digits)
    /// </summary>
    public string? GTIN { get; set; }

    /// <summary>
    /// Serial Number (unique identifier for the product)
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Batch/Lot Number
    /// </summary>
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Expiry Date in MM/dd/yyyy format
    /// </summary>
    public string? ExpiryDate { get; set; }

    /// <summary>
    /// Production Date (if available)
    /// </summary>
    public string? ProductionDate { get; set; }

    /// <summary>
    /// Indicates if the parsing was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Error message if parsing failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Raw parsed data as dictionary
    /// </summary>
    public Dictionary<string, string> RawData { get; set; } = new();
}
