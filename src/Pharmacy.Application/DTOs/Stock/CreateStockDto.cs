using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Stock;

/// <summary>
/// DTO for creating or updating Stock
/// </summary>
public class CreateStockDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Branch ID is required")]
    public Guid BranchId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Quantity must be a positive value")]
    public decimal? Quantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Reserved quantity must be a positive value")]
    public decimal? ReservedQuantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Average cost must be a positive value")]
    public decimal? AverageCost { get; set; }
    
    public int? Status { get; set; }
}
