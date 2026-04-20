using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("GenericNames")]
public class GenericName : BaseEntity
{
    [Required]
    [MaxLength(2000)]
    public string NameEN { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string NameAR { get; set; } = string.Empty;

    // Navigation: Products that reference this generic name
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
