using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Product;

/// <summary>
/// Request DTO for parsing GS1 barcode data
/// </summary>
public class BarcodeParseRequestDto
{
    /// <summary>
    /// Raw barcode string (GS1 DataMatrix/QR code format)
    /// </summary>
    [Required(ErrorMessage = "Barcode input is required")]
    [StringLength(500, ErrorMessage = "Barcode cannot exceed 500 characters")]
    public string BarcodeInput { get; set; } = string.Empty;
}
