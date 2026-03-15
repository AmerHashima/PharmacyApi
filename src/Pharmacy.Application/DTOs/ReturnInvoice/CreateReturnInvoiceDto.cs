using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.ReturnInvoice;

/// <summary>
/// DTO for creating a new Return Invoice
/// </summary>
public class CreateReturnInvoiceDto
{
    [Required(ErrorMessage = "Original Invoice ID is required")]
    public Guid OriginalInvoiceId { get; set; }

    [Required(ErrorMessage = "Branch ID is required")]
    public Guid BranchId { get; set; }

    [MaxLength(200)]
    public string? CustomerName { get; set; }

    [MaxLength(50)]
    public string? CustomerPhone { get; set; }

    [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
    public decimal? DiscountPercent { get; set; }

    public DateTime? ReturnDate { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public Guid? CashierId { get; set; }

    public Guid? ReturnReasonId { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "At least one item is required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<CreateReturnInvoiceItemDto> Items { get; set; } = new();
}

/// <summary>
/// DTO for creating a return invoice line item
/// </summary>
public class CreateReturnInvoiceItemDto
{
    /// <summary>
    /// FK to the original sales invoice item (optional)
    /// </summary>
    public Guid? OriginalInvoiceItemId { get; set; }

    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
    public decimal? DiscountPercent { get; set; }

    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
