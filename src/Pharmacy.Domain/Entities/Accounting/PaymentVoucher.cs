using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("PaymentVouchers", Schema = "Accounting")]
public class PaymentVoucher : BaseEntity
{
    [MaxLength(100)]
    public string? VoucherNumber { get; set; }

    public DateTime VoucherDate { get; set; }

    public Guid? StakeholderId { get; set; }

    [ForeignKey(nameof(StakeholderId))]
    public virtual Stakeholder? Stakeholder { get; set; }

    public Guid? CashBoxId { get; set; }

    [ForeignKey(nameof(CashBoxId))]
    public virtual CashBox? CashBox { get; set; }

    public Guid? BankAccountId { get; set; }

    [ForeignKey(nameof(BankAccountId))]
    public virtual BankAccount? BankAccount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public string? Notes { get; set; }

    public Guid? JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public virtual JournalEntry? JournalEntry { get; set; }
}
