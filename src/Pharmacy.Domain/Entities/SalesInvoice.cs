using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a sales transaction/invoice from POS.
/// Contains header information for a sale.
/// </summary>
[Table("SalesInvoices")]
public class SalesInvoice : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;

    /// <summary>
    /// Customer name (optional for walk-in customers)
    /// </summary>
    [MaxLength(200)]
    public string? CustomerName { get; set; }

    /// <summary>
    /// Customer phone number
    /// </summary>
    [MaxLength(50)]
    public string? CustomerPhone { get; set; }

    /// <summary>
    /// Customer email
    /// </summary>
    [MaxLength(100)]
    public string? CustomerEmail { get; set; }

    /// <summary>
    /// Total amount before discount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? SubTotal { get; set; }

    /// <summary>
    /// Discount percentage
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? DiscountPercent { get; set; }

    /// <summary>
    /// Discount amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// Tax amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// Final total amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// Amount paid by customer
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// Change returned to customer
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? ChangeAmount { get; set; }

    /// <summary>
    /// Invoice date and time
    /// </summary>
    public DateTime? InvoiceDate { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Payment method (Cash, Card, Credit, etc.)
    /// </summary>
    public Guid? PaymentMethodId { get; set; }

    [ForeignKey(nameof(PaymentMethodId))]
    public virtual AppLookupDetail? PaymentMethod { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Invoice status (Pending, Completed, Cancelled, Refunded)
    /// </summary>
    public Guid? InvoiceStatusId { get; set; }

    [ForeignKey(nameof(InvoiceStatusId))]
    public virtual AppLookupDetail? InvoiceStatus { get; set; }

    /// <summary>
    /// Cashier who processed the sale
    /// </summary>
    public Guid? CashierId { get; set; }

    [ForeignKey(nameof(CashierId))]
    public virtual SystemUser? Cashier { get; set; }

    /// <summary>
    /// Prescription number if applicable
    /// </summary>
    [MaxLength(50)]
    public string? PrescriptionNumber { get; set; }

    /// <summary>
    /// Doctor name for prescription sales
    /// </summary>
    [MaxLength(200)]
    public string? DoctorName { get; set; }

    /// <summary>
    /// Notes or remarks
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation Properties
    public virtual ICollection<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
