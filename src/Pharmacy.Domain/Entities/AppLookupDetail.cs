using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("AppLookupDetail")]
public class AppLookupDetail : BaseEntity
{
    [Required]
    public Guid MasterID { get; set; }

    [ForeignKey(nameof(MasterID))]
    public virtual AppLookupMaster Master { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string ValueCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ValueNameAr { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ValueNameEn { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; } = true;

    // DO NOT add CreatedByUser or UpdatedByUser navigation properties
    // This causes EF Core relationship ambiguity issues
 
}