using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("Accounts", Schema = "Accounting")]
public class Account : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string AccountCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string AccountNameAr { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? AccountNameEn { get; set; }

    public Guid? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public virtual Account? Parent { get; set; }

    public virtual ICollection<Account> Children { get; set; } = new List<Account>();

    public int AccountLevel { get; set; } = 1;

    /// <summary>FK to AppLookupDetail — ACCOUNT_TYPE (Assets, Liabilities, Equity, Revenue, Expense).</summary>
    public Guid? AccountTypeId { get; set; }

    [ForeignKey(nameof(AccountTypeId))]
    public virtual AppLookupDetail? AccountType { get; set; }

    /// <summary>FK to AppLookupDetail — ACCOUNT_NATURE (Debit / Credit).</summary>
    public Guid? NatureId { get; set; }

    [ForeignKey(nameof(NatureId))]
    public virtual AppLookupDetail? Nature { get; set; }

    public bool IsLeaf { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
