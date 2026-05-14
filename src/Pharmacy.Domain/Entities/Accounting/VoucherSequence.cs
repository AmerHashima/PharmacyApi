using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Entities.Accounting;

/// <summary>
/// Stores the last used sequence number per branch / voucher-type / year.
/// Managed exclusively by <c>VoucherNumberService</c> via atomic MERGE statements.
/// </summary>
[Table("VoucherSequences", Schema = "Accounting")]
public class VoucherSequence : BaseEntity
{
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    [Required]
    [MaxLength(10)]
    public string VoucherType { get; set; } = string.Empty;

    public int Year { get; set; }

    public int LastSequence { get; set; }
}
