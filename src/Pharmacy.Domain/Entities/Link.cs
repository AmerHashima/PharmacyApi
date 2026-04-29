using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("Links")]
public class Link : BaseEntity
{
    [MaxLength(300)]
    public string? NameAr { get; set; }

    [MaxLength(300)]
    public string? NameEn { get; set; }

    /// <summary>Icon identifier (front-end icon code).</summary>
    public int? Icon { get; set; }

    /// <summary>Link type: 3 = report, others = navigation.</summary>
    public int? Type { get; set; }

    public bool? Active { get; set; }

    [MaxLength(500)]
    public string? Path { get; set; }

    /// <summary>Key used to look up the report definition.</summary>
    [MaxLength(200)]
    public string? ReportsKey { get; set; }

    public bool? InViewList { get; set; }

    public virtual ICollection<ReportParameter> ReportParameters { get; set; } = new List<ReportParameter>();
}
