namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// A single drug record returned from SFDA RSD DrugListService.
/// Field names mirror the XML element names in the response.
/// </summary>
public class DrugListItemDto
{
    public string? GTIN { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? DrugName { get; set; }
    public string? GenericName { get; set; }
    public string? StrengthValue { get; set; }
    public string? StrengthValueUnit { get; set; }
    public string? PackageType { get; set; }
    public string? PackageSize { get; set; }
    public string? DosageForm { get; set; }
    public string? DrugStatus { get; set; }
    public string? MarketingStatus { get; set; }
    public string? LegalStatus { get; set; }
    public string? DomainId { get; set; }
    public string? Price { get; set; }
    public string? Volume { get; set; }
    public string? UnitOfVolume { get; set; }
    public bool? IsExportable { get; set; }
    public bool? IsImportable { get; set; }
    public List<DrugListSupplierDto> Suppliers { get; set; } = [];
}
