using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Stock;

/// <summary>
/// DTO for updating stock quantity
/// </summary>
public class UpdateStockQuantityDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Branch ID is required")]
    public Guid BranchId { get; set; }

    /// <summary>
    /// The quantity change (positive for increase, negative for decrease)
    /// </summary>
    [Required(ErrorMessage = "Quantity change is required")]
    public decimal QuantityChange { get; set; }

    /// <summary>
    /// Reference for the stock adjustment
    /// </summary>
    [MaxLength(100)]
    public string? Reference { get; set; }

    /// <summary>
    /// Notes for the adjustment
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
}
