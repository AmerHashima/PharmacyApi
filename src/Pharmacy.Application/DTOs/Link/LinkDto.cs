namespace Pharmacy.Application.DTOs.Link;

public class LinkDto
{
    public Guid Oid { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public int? Icon { get; set; }
    public int? Type { get; set; }
    public bool? Active { get; set; }
    public string? Path { get; set; }
    public string? ReportsKey { get; set; }
    public bool? InViewList { get; set; }
    public DateTime? CreatedAt { get; set; }

    public List<ReportParameterDto> ReportParameters { get; set; } = new();
}
