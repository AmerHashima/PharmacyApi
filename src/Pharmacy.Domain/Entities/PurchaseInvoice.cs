using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("PurchaseInvoices")]
public class PurchaseInvoice : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string PurchaseInvoiceNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? SupplierInvoiceNumber { get; set; }

    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    [Required]
    public Guid SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public virtual Stakeholder? Supplier { get; set; }

    public DateTime PurchaseDate { get; set; }

    public Guid? StockTransactionId { get; set; }

    [ForeignKey(nameof(StockTransactionId))]
    public virtual StockTransaction? StockTransaction { get; set; }

    public Guid? JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public virtual JournalEntry? JournalEntry { get; set; }

    public Guid? FiscalYearId { get; set; }

    [ForeignKey(nameof(FiscalYearId))]
    public virtual FiscalYear? FiscalYear { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? DiscountPercent { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PaidAmount { get; set; }

    public Guid? InvoiceStatusId { get; set; }

    [ForeignKey(nameof(InvoiceStatusId))]
    public virtual AppLookupDetail? InvoiceStatus { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public virtual ICollection<PurchaseInvoicePayment> Payments { get; set; } = new List<PurchaseInvoicePayment>();
}
