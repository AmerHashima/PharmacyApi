using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("BankAccounts", Schema = "Accounting")]
public class BankAccount : BaseEntity
{
    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(300)]
    public string? NameAr { get; set; }

    [MaxLength(300)]
    public string? NameEn { get; set; }

    [MaxLength(100)]
    public string? AccountNumber { get; set; }

    [MaxLength(200)]
    public string? IBAN { get; set; }

    public Guid? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }


    // ── Accounting ─────────────────────────────────────────────────────────

    /// <summary>FK to Account — the parent account this customer is linked to.</summary>
    public Guid? ParentAccountId { get; set; }

    [ForeignKey(nameof(ParentAccountId))]
    public virtual Account? ParentAccount { get; set; }

    /// <summary>FK to Account — the child account created specifically for this customer.</summary>
    public Guid? ChildAccountId { get; set; }

    [ForeignKey(nameof(ChildAccountId))]
    public virtual Account? ChildAccount { get; set; }

    /// <summary>FK to AppLookupDetail — CURRENCY_CODE (SAR, USD, EUR, GBP).</summary>
    public Guid? CurrencyCodeId { get; set; }

    [ForeignKey(nameof(CurrencyCodeId))]
    public virtual AppLookupDetail? CurrencyCode { get; set; }

    public bool IsActive { get; set; } = true;
}
