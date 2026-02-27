using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for creating a new StockTransactionDetail
/// </summary>
public class CreateStockTransactionDetailDto
{
    //[Required(ErrorMessage = "Stock transaction ID is required")]
    public Guid StockTransactionId { get; set; }

    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    [StringLength(50, ErrorMessage = "GTIN cannot exceed 50 characters")]
    public string? Gtin { get; set; }

    [StringLength(50, ErrorMessage = "Batch number cannot exceed 50 characters")]
    public string? BatchNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [StringLength(100, ErrorMessage = "Serial number cannot exceed 100 characters")]
    public string? SerialNumber { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Unit cost must be a positive value")]
    public decimal? UnitCost { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Total cost must be a positive value")]
    public decimal? TotalCost { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Line number must be greater than 0")]
    public int LineNumber { get; set; } = 1;

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}
