using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pharmacy.Application.DTOs.Product;

/// <summary>
/// DTO for updating product information
/// </summary>
public class UpdateProductDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public Guid Oid { get; set; }

    public string? Barcode { get; set; }

    [StringLength(14, ErrorMessage = "GTIN must be 14 digits")]
    public string? GTIN { get; set; }

    [Required(ErrorMessage = "Drug name is required")]
    [StringLength(500, ErrorMessage = "Drug name cannot exceed 500 characters")]
    public string DrugName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Arabic drug name cannot exceed 500 characters")]
    public string DrugNameAr { get; set; } = string.Empty;

    public Guid? VatTypeId { get; set; }

    [StringLength(500, ErrorMessage = "Generic name cannot exceed 500 characters")]
    public string? GenericName { get; set; }

    public Guid? ProductGroupId { get; set; }

    public Guid? ProductTypeId { get; set; }

    public Guid? PackageTypeId { get; set; }

    public Guid? DosageFormId { get; set; }

    [StringLength(50)]
    public string? StrengthValue { get; set; }

    [StringLength(20)]
    public string? StrengthUnit { get; set; }

    [StringLength(50)]
    public string? PackageSize { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    public decimal? Price { get; set; }

    [StringLength(100)]
    public string? RegistrationNumber { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Volume must be a positive value")]
    public decimal? Volume { get; set; }

    [StringLength(20)]
    public string? UnitOfVolume { get; set; }

    public bool? IsExportable { get; set; }

    public bool? IsImportable { get; set; }

    [StringLength(50)]
    public string? DrugStatus { get; set; }

    [StringLength(50)]
    public string? MarketingStatus { get; set; }

    [StringLength(50)]
    public string? LegalStatus { get; set; }

    [StringLength(50)]
    public string? DomainId { get; set; }

    [StringLength(500)]
    public string? Manufacturer { get; set; }

    [StringLength(200)]
    public string? CountryOfOrigin { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Minimum stock level must be a positive value")]
    public decimal? MinStockLevel { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Maximum stock level must be a positive value")]
    public decimal? MaxStockLevel { get; set; }

    /// <summary>
    /// Status field from JSON (optional, for compatibility)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Status { get; set; }
}