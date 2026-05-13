using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("ReceiptVouchers", Schema = "Accounting")]
public class ReceiptVoucher : BaseEntity
{
    [MaxLength(100)]
    public string? VoucherNumber { get; set; }

    public DateTime VoucherDate { get; set; }

    public Guid? FiscalYearId { get; set; }

    [ForeignKey(nameof(FiscalYearId))]
    public virtual FiscalYear? FiscalYear { get; set; }

    public Guid? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    public Guid? CashBoxId { get; set; }

    [ForeignKey(nameof(CashBoxId))]
    public virtual CashBox? CashBox { get; set; }

    public Guid? BankAccountId { get; set; }

    [ForeignKey(nameof(BankAccountId))]
    public virtual BankAccount? BankAccount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public Guid? JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public virtual JournalEntry? JournalEntry { get; set; }

    public virtual ICollection<ReceiptVoucherDetail> Details { get; set; } = new List<ReceiptVoucherDetail>();
}
