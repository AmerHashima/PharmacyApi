using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Link;

public class CreateLinkDto
{
    [Required]
    [MaxLength(300)]
    public string? NameAr { get; set; }

    [Required]
    [MaxLength(300)]
    public string? NameEn { get; set; }

    public int? Icon { get; set; }

    public int? Type { get; set; }

    public bool? Active { get; set; }

    [MaxLength(500)]
    public string? Path { get; set; }

    [MaxLength(200)]
    public string? ReportsKey { get; set; }

    public bool? InViewList { get; set; }

    public List<CreateReportParameterDto> ReportParameters { get; set; } = new();
}
