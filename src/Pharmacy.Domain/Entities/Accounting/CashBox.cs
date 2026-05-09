using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("CashBoxes", Schema = "Accounting")]
public class CashBox : BaseEntity
{
    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(300)]
    public string? NameAr { get; set; }

    [MaxLength(300)]
    public string? NameEn { get; set; }

    public Guid? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    public Guid? AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public virtual Account? Account { get; set; }

    public bool IsActive { get; set; } = true;
}
