using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for creating a stock transaction with its detail lines
/// </summary>
public class CreateStockTransactionWithDetailsDto
{
    [Required(ErrorMessage = "Transaction type is required")]
    public Guid TransactionTypeId { get; set; }

    public Guid? FromBranchId { get; set; }

    public Guid? ToBranchId { get; set; }

    [StringLength(100, ErrorMessage = "Reference number cannot exceed 100 characters")]
    public string? ReferenceNumber { get; set; }

    [StringLength(100)]
    public string? NotificationId { get; set; }

    [Required(ErrorMessage = "Transaction date is required")]
    public DateTime TransactionDate { get; set; }

    public Guid? SupplierId { get; set; }

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// Detail lines for this transaction
    /// </summary>
    [Required(ErrorMessage = "At least one detail line is required")]
    [MinLength(1, ErrorMessage = "At least one detail line is required")]
    public List<CreateStockTransactionDetailForMasterDto> Details { get; set; } = new();
}
