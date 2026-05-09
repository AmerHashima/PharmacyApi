using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("CostCenters", Schema = "Accounting")]
public class CostCenter : BaseEntity
{
    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(300)]
    public string? NameAr { get; set; }

    [MaxLength(300)]
    public string? NameEn { get; set; }

    public Guid? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public virtual CostCenter? Parent { get; set; }

    public virtual ICollection<CostCenter> Children { get; set; } = new List<CostCenter>();

    public bool IsActive { get; set; } = true;
}
