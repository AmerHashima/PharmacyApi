using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Records all stock movements: IN (receiving), OUT (dispensing), TRANSFER (between branches).
/// Provides complete audit trail for inventory changes.
/// </summary>
[Table("StockTransactions")]
public class StockTransaction : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Source branch for OUT and TRANSFER transactions
    /// </summary>
    public Guid? FromBranchId { get; set; }

    [ForeignKey(nameof(FromBranchId))]
    public virtual Branch? FromBranch { get; set; }

    /// <summary>
    /// Destination branch for IN and TRANSFER transactions
    /// </summary>
    public Guid? ToBranchId { get; set; }

    [ForeignKey(nameof(ToBranchId))]
    public virtual Branch? ToBranch { get; set; }

    /// <summary>
    /// Quantity being transacted
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Transaction type (IN, OUT, TRANSFER, ADJUSTMENT, RETURN)
    /// </summary>
    public Guid? TransactionTypeId { get; set; }

    [ForeignKey(nameof(TransactionTypeId))]
    public virtual AppLookupDetail? TransactionType { get; set; }

    /// <summary>
    /// Reference number (e.g., PO number, invoice number, transfer request number)
    /// </summary>
    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Date and time of the transaction
    /// </summary>
    public DateTime? TransactionDate { get; set; }

    /// <summary>
    /// Unit cost at time of transaction
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// Total value of the transaction
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalValue { get; set; }

    /// <summary>
    /// Batch number for traceability
    /// </summary>
    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Expiry date of the batch
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Supplier for IN transactions
    /// </summary>
    public Guid? SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public virtual Stakeholder? Supplier { get; set; }

    /// <summary>
    /// Notes or remarks about the transaction
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// FK to SalesInvoice - links OUT transactions to sales
    /// </summary>
    public Guid? SalesInvoiceId { get; set; }

    [ForeignKey(nameof(SalesInvoiceId))]
    public virtual SalesInvoice? SalesInvoice { get; set; }
}
