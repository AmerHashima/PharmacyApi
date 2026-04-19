using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Defines invoice types per branch (e.g. POS Invoice, Supplier Invoice, etc.)
/// Rows with BranchId = null act as global defaults/templates.
/// </summary>
[Table("InvoiceSetups")]
public class InvoiceSetup : BaseEntity
{
    /// <summary>Arabic name of the invoice type</summary>
    [Required]
    [MaxLength(200)]
    public string NameAr { get; set; } = string.Empty;

    /// <summary>English name of the invoice type</summary>
    [Required]
    [MaxLength(200)]
    public string NameEn { get; set; } = string.Empty;

    /// <summary>Format / prefix code used when generating invoice numbers (e.g. "PosInv")</summary>
    [Required]
    [MaxLength(50)]
    public string Format { get; set; } = string.Empty;

    /// <summary>Current invoice number counter — incremented on each new invoice of this type</summary>
    public int NumberValue { get; set; } = 1;

    /// <summary>
    /// FK to Branch. Null = global template row (not branch-specific).
    /// </summary>
    public Guid? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }
}
