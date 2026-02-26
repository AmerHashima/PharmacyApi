namespace Pharmacy.Application.DTOs.Product;

public class CreateProductDto
{
    public string? Barcode { get; set; }
    public string? GTIN { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string DrugNameAr { get; set; } = string.Empty;
    public Guid? VatTypeId { get; set; }
    public string? GenericName { get; set; }
    public Guid? ProductGroupId { get; set; }
    public Guid? ProductTypeId { get; set; }
    public string? StrengthValue { get; set; }
    public string? StrengthUnit { get; set; }
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
}