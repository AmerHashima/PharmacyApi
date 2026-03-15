using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a return/refund invoice.
/// Contains header information for a return against an original sales invoice.
/// </summary>
[Table("ReturnInvoices")]
public class ReturnInvoice : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string ReturnNumber { get; set; } = string.Empty;

    /// <summary>
    /// FK to the original sales invoice being returned
    /// </summary>
    [Required]
    public Guid OriginalInvoiceId { get; set; }

    [ForeignKey(nameof(OriginalInvoiceId))]
    public virtual SalesInvoice OriginalInvoice { get; set; } = null!;

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
    /// Final total refund amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// Amount refunded to customer
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? RefundAmount { get; set; }

    /// <summary>
    /// Return date and time
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Payment method for refund (Cash, Card, Credit, etc.)
    /// </summary>
    public Guid? PaymentMethodId { get; set; }

    [ForeignKey(nameof(PaymentMethodId))]
    public virtual AppLookupDetail? PaymentMethod { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Invoice status (Pending, Completed, Cancelled)
    /// </summary>
    public Guid? InvoiceStatusId { get; set; }

    [ForeignKey(nameof(InvoiceStatusId))]
    public virtual AppLookupDetail? InvoiceStatus { get; set; }

    /// <summary>
    /// Cashier who processed the return
    /// </summary>
    public Guid? CashierId { get; set; }

    [ForeignKey(nameof(CashierId))]
    public virtual SystemUser? Cashier { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Return reason
    /// </summary>
    public Guid? ReturnReasonId { get; set; }

    [ForeignKey(nameof(ReturnReasonId))]
    public virtual AppLookupDetail? ReturnReason { get; set; }

    /// <summary>
    /// Notes or remarks
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation Properties
    public virtual ICollection<ReturnInvoiceItem> Items { get; set; } = new List<ReturnInvoiceItem>();
    public virtual ICollection<StockTransactionReturn> StockTransactionReturns { get; set; } = new List<StockTransactionReturn>();
}
