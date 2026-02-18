using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for creating a Stock Transfer transaction (between branches)
/// </summary>
public class CreateStockTransferDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "From Branch ID is required")]
    public Guid FromBranchId { get; set; }

    [Required(ErrorMessage = "To Branch ID is required")]
    public Guid ToBranchId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    public DateTime? TransactionDate { get; set; }

    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
