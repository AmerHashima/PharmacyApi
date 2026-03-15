using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Records stock return movements linked to return invoices.
/// Master/Header entity - contains return transaction metadata.
/// Detail lines are in StockTransactionReturnDetails.
/// </summary>
[Table("StockTransactionReturns")]
public class StockTransactionReturn : BaseEntity
{
    /// <summary>
    /// Source branch for the return (where stock is being returned from)
    /// </summary>
    public Guid? FromBranchId { get; set; }

    [ForeignKey(nameof(FromBranchId))]
    public virtual Branch? FromBranch { get; set; }

    /// <summary>
    /// Destination branch for the return (where stock is going back to)
    /// </summary>
    public Guid? ToBranchId { get; set; }

    [ForeignKey(nameof(ToBranchId))]
    public virtual Branch? ToBranch { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Transaction type (RETURN, etc.)
    /// </summary>
    public Guid? TransactionTypeId { get; set; }

    [ForeignKey(nameof(TransactionTypeId))]
    public virtual AppLookupDetail? TransactionType { get; set; }

    /// <summary>
    /// Reference number for the return transaction
    /// </summary>
    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    [MaxLength(100)]
    public string? NotificationId { get; set; }

    /// <summary>
    /// Date and time of the return transaction
    /// </summary>
    public DateTime? TransactionDate { get; set; }

    /// <summary>
    /// Total value of all items in this return transaction
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalValue { get; set; }

    /// <summary>
    /// Supplier for supplier returns
    /// </summary>
    public Guid? SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public virtual Stakeholder? Supplier { get; set; }

    /// <summary>
    /// Notes or remarks about the return transaction
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// FK to ReturnInvoice - links this stock return to the originating return invoice
    /// </summary>
    public Guid? ReturnInvoiceId { get; set; }

    [ForeignKey(nameof(ReturnInvoiceId))]
    public virtual ReturnInvoice? ReturnInvoice { get; set; }

    /// <summary>
    /// FK to original StockTransaction (optional - links to the original stock movement being reversed)
    /// </summary>
    public Guid? OriginalTransactionId { get; set; }

    [ForeignKey(nameof(OriginalTransactionId))]
    public virtual StockTransaction? OriginalTransaction { get; set; }

    /// <summary>
    /// Status of the return transaction (Draft, Approved, Completed, Cancelled)
    /// </summary>
    [MaxLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// User who approved the return transaction
    /// </summary>
    public Guid? ApprovedBy { get; set; }

    /// <summary>
    /// Date when return transaction was approved
    /// </summary>
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// Collection of detail lines for this return transaction
    /// </summary>
    public virtual ICollection<StockTransactionReturnDetail> Details { get; set; } = new List<StockTransactionReturnDetail>();
}
