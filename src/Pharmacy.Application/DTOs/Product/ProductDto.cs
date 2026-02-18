namespace Pharmacy.Application.DTOs.Product;

/// <summary>
/// DTO for reading Product data
/// </summary>
public class ProductDto
{
    public Guid Oid { get; set; }
    public string? GTIN { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string? GenericName { get; set; }
    public Guid? ProductTypeId { get; set; }
    public string? ProductTypeName { get; set; }
    public string? StrengthValue { get; set; }
    public string? StrengthUnit { get; set; }
    public string? FullStrength => string.IsNullOrEmpty(StrengthValue) ? null : $"{StrengthValue} {StrengthUnit}".Trim();
    public string? PackageType { get; set; }
    public string? PackageSize { get; set; }
    public decimal? Price { get; set; }
    public string? RegistrationNumber { get; set; }
    public decimal? Volume { get; set; }
    public string? UnitOfVolume { get; set; }
    public bool? IsExportable { get; set; }
    public bool? IsImportable { get; set; }
    public string? DrugStatus { get; set; }
    public string? MarketingStatus { get; set; }
    public string? LegalStatus { get; set; }
    public string? DomainId { get; set; }
    public string? Manufacturer { get; set; }
    public string? CountryOfOrigin { get; set; }
    public decimal? MinStockLevel { get; set; }
    public decimal? MaxStockLevel { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
