using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("FiscalYears", Schema = "Accounting")]
public class FiscalYear : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string NameAR { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NameEn { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsClosed { get; set; } = false;
}
