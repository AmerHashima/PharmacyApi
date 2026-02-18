namespace Pharmacy.Application.DTOs.AppLookup;

public class AppLookupMasterDto
{
    public Guid Oid { get; set; }
    public string LookupCode { get; set; } = string.Empty;
    public string LookupNameAr { get; set; } = string.Empty;
    public string LookupNameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<AppLookupDetailDto> LookupDetails { get; set; } = new List<AppLookupDetailDto>();
}