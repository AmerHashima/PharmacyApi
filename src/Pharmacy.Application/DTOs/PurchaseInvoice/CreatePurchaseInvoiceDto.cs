using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.PurchaseInvoice;

public class CreatePurchaseInvoiceDto
{
    [MaxLength(100)]
    public string? SupplierInvoiceNumber { get; set; }

    [Required]
    public Guid BranchId { get; set; }

    [Required]
    public Guid SupplierId { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    public Guid? StockTransactionId { get; set; }
    public Guid? FiscalYearId { get; set; }

    [Required]
    public decimal SubTotal { get; set; }

    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public Guid? InvoiceStatusId { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public List<CreatePurchaseInvoicePaymentDto> Payments { get; set; } = new();
}

public class CreatePurchaseInvoicePaymentDto
{
    public Guid? PaymentVoucherId { get; set; }

    [Required]
    public Guid PaymentMethodId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    [MaxLength(100)]
    public string? TransactionId { get; set; }

    [MaxLength(100)]
    public string? ChequeNumber { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Notes { get; set; }
}
