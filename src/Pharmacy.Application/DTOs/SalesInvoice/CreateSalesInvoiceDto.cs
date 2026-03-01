using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.SalesInvoice;

/// <summary>
/// DTO for creating a new Sales Invoice (POS transaction)
/// </summary>
public class CreateSalesInvoiceDto
{
    [Required(ErrorMessage = "Branch ID is required")]
    public Guid BranchId { get; set; }

    [MaxLength(200)]
    public string? CustomerName { get; set; }

    [MaxLength(50)]
    public string? CustomerPhone { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100)]
    public string? CustomerEmail { get; set; }

    [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
    public decimal? DiscountPercent { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public Guid? CashierId { get; set; }

    [MaxLength(50)]
    public string? PrescriptionNumber { get; set; }

    [MaxLength(200)]
    public string? DoctorName { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "At least one item is required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<CreateSalesInvoiceItemDto> Items { get; set; } = new();
}

/// <summary>
/// DTO for creating a sales invoice line item
/// </summary>
public class CreateSalesInvoiceItemDto
{
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
    public string? SerialNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
