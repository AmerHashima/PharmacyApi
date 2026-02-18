using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("AppLookupMaster")]
public class AppLookupMaster : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string LookupCode { get; set; } = string.Empty; // e.g. GENDER, MARITAL_STATUS

    [Required]
    [MaxLength(100)]
    public string LookupNameAr { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LookupNameEn { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    public bool IsSystem { get; set; } = false; // System / Custom

    // Navigation Properties
    public virtual ICollection<AppLookupDetail> LookupDetails { get; set; } = new List<AppLookupDetail>();
}