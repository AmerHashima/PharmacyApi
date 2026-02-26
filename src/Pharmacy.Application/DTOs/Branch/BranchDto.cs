namespace Pharmacy.Application.DTOs.Branch;

/// <summary>
/// DTO for reading Branch data
/// </summary>
public class BranchDto
{
    public Guid Oid { get; set; }
    public string BranchCode { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string? GLN { get; set; }
    public string? CRN { get; set; }
    public string? VatNumber { get; set; }
    public Guid? IdentifyLookupId { get; set; }
    public string? IdentifyLookupName { get; set; }
    public string? IdentifyValue { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    public string? StreetName { get; set; }
    public string? BuildingNumber { get; set; }
    public string? CitySubdivisionName { get; set; }
    public string? CityName { get; set; }
    public string? PostalZone { get; set; }
    public string? RegistrationName { get; set; }
    public DateTime CreatedAt { get; set; }
}
