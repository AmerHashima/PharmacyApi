using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for creating a Stock IN transaction (receiving inventory)
/// </summary>
public class CreateStockInDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "To Branch ID is required")]
    public Guid ToBranchId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    public DateTime? TransactionDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Unit cost must be a positive value")]
    public decimal? UnitCost { get; set; }

    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public Guid? SupplierId { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
