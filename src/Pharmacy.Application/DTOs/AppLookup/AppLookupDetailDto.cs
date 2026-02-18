namespace Pharmacy.Application.DTOs.AppLookup;

public class AppLookupDetailDto
{
    public Guid Oid { get; set; }
    public Guid LookupMasterID { get; set; }
    public string ValueCode { get; set; } = string.Empty;
    public string ValueNameAr { get; set; } = string.Empty;
    public string ValueNameEn { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsDefault { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? MasterLookupCode { get; set; } // For reference
}