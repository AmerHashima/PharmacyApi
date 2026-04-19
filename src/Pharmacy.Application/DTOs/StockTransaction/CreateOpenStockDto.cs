using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// Request payload for resolving Open Stock items.
/// No data is written to the database — the list is resolved and returned.
/// </summary>
public class ResolveOpenStockDto
{
    [Required(ErrorMessage = "At least one item is required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<OpenStockItemDto> Items { get; set; } = new();
}
