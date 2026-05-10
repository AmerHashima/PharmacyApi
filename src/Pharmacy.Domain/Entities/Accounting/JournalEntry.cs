using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("JournalEntries", Schema = "Accounting")]
public class JournalEntry : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string EntryNumber { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; }

    public Guid? FiscalYearId { get; set; }

    [ForeignKey(nameof(FiscalYearId))]
    public virtual FiscalYear? FiscalYear { get; set; }

    public Guid? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    public string? Description { get; set; }

    public Guid? ReferenceTypeId { get; set; }

    [ForeignKey(nameof(ReferenceTypeId))]
    public virtual AppLookupDetail? ReferenceType { get; set; }

    public Guid? ReferenceId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDebit { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCredit { get; set; } = 0;

    public bool IsPosted { get; set; } = false;
    public bool IsReversed { get; set; } = false;

    public virtual ICollection<JournalEntryDetail> Details { get; set; } = new List<JournalEntryDetail>();
}
