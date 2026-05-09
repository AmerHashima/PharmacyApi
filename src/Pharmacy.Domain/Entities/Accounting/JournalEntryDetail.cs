using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("JournalEntryDetails", Schema = "Accounting")]
public class JournalEntryDetail : BaseEntity
{
    public Guid JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public virtual JournalEntry? JournalEntry { get; set; }

    public Guid AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public virtual Account? Account { get; set; }

    public Guid? CostCenterId { get; set; }

    [ForeignKey(nameof(CostCenterId))]
    public virtual CostCenter? CostCenter { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Debit { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Credit { get; set; } = 0;
}
