namespace Pharmacy.Application.DTOs.Link;

public class ReportParameterDto
{
    public Guid Oid { get; set; }
    public Guid LinksOid { get; set; }
    public string? Name { get; set; }
    public bool? Active { get; set; }
    public int? Type { get; set; }
    public string? DefaultValue { get; set; }
}
