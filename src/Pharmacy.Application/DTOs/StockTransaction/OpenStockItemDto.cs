using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// A single line item in an Open Stock request.
/// The product is resolved by GTIN or Barcode.
/// </summary>
public class OpenStockItemDto
{
    /// <summary>
    /// GTIN or Barcode used to look up the product
    /// </summary>
    [Required(ErrorMessage = "GTIN or Barcode is required")]
    [MaxLength(50)]
    public string GtinOrBarcode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.001, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    [Required(ErrorMessage = "Unit cost is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Unit cost must be 0 or greater")]
    public decimal UnitCost { get; set; }

    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
