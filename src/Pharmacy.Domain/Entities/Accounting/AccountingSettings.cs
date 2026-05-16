using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

/// <summary>
/// Per-branch accounting settings.
/// Maps each accounting event to the correct Chart-of-Accounts entry.
/// </summary>
[Table("AccountingSettings", Schema = "Accounting")]
public class AccountingSettings : BaseEntity
{
    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    /// <summary>حساب المبيعات — credited on every sale.</summary>
    public Guid? SalesAccountId { get; set; }

    [ForeignKey(nameof(SalesAccountId))]
    public virtual Account? SalesAccount { get; set; }

    /// <summary>حساب الضريبة — VAT payable.</summary>
    public Guid? VatAccountId { get; set; }

    [ForeignKey(nameof(VatAccountId))]
    public virtual Account? VatAccount { get; set; }

    /// <summary>حساب الخصم الممنوح — discount granted to customers.</summary>
    public Guid? DiscountAccountId { get; set; }

    [ForeignKey(nameof(DiscountAccountId))]
    public virtual Account? DiscountAccount { get; set; }

    /// <summary>تكلفة البضاعة المباعة — COGS.</summary>
    public Guid? CogsAccountId { get; set; }

    [ForeignKey(nameof(CogsAccountId))]
    public virtual Account? CogsAccount { get; set; }

    /// <summary>حساب المخزون — Inventory asset account.</summary>
    public Guid? InventoryAccountId { get; set; }

    [ForeignKey(nameof(InventoryAccountId))]
    public virtual Account? InventoryAccount { get; set; }

    /// <summary>حساب الصندوق الافتراضي — default cash box account.</summary>
    public Guid? CashAccountId { get; set; }

    [ForeignKey(nameof(CashAccountId))]
    public virtual Account? CashAccount { get; set; }

    /// <summary>حساب البنك الافتراضي — default bank account.</summary>
    public Guid? BankAccountId { get; set; }

    [ForeignKey(nameof(BankAccountId))]
    public virtual Account? BankAccount { get; set; }

    /// <summary>حساب العملاء الآجل — accounts receivable.</summary>
    public Guid? ReceivableAccountId { get; set; }

    [ForeignKey(nameof(ReceivableAccountId))]
    public virtual Account? ReceivableAccount { get; set; }
}
