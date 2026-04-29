using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("ReportParameters")]
public class ReportParameter : BaseEntity
{
    [Required]
    public Guid LinksOid { get; set; }

    [ForeignKey(nameof(LinksOid))]
    public virtual Link? Link { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    public bool? Active { get; set; }

    /// <summary>Parameter data type (e.g. 1=string, 2=date, 3=int).</summary>
    public int? Type { get; set; }

    [MaxLength(500)]
    public string? DefaultValue { get; set; }
}
