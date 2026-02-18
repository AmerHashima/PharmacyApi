using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Product;

/// <summary>
/// DTO for creating a new Product
/// </summary>
public class CreateProductDto
{
    [MaxLength(14, ErrorMessage = "GTIN cannot exceed 14 characters")]
    public string? GTIN { get; set; }

    [Required(ErrorMessage = "Drug name is required")]
    [MaxLength(500, ErrorMessage = "Drug name cannot exceed 500 characters")]
    public string DrugName { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Generic name cannot exceed 500 characters")]
    public string? GenericName { get; set; }

    public Guid? ProductTypeId { get; set; }

    [MaxLength(50, ErrorMessage = "Strength value cannot exceed 50 characters")]
    public string? StrengthValue { get; set; }

    [MaxLength(50, ErrorMessage = "Strength unit cannot exceed 50 characters")]
    public string? StrengthUnit { get; set; }

    [MaxLength(100, ErrorMessage = "Package type cannot exceed 100 characters")]
    public string? PackageType { get; set; }

    [MaxLength(50, ErrorMessage = "Package size cannot exceed 50 characters")]
    public string? PackageSize { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    public decimal? Price { get; set; }

    [MaxLength(50, ErrorMessage = "Registration number cannot exceed 50 characters")]
    public string? RegistrationNumber { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Volume must be a positive value")]
    public decimal? Volume { get; set; }

    [MaxLength(50, ErrorMessage = "Unit of volume cannot exceed 50 characters")]
    public string? UnitOfVolume { get; set; }

    public bool? IsExportable { get; set; }
    public bool? IsImportable { get; set; }

    [MaxLength(50, ErrorMessage = "Drug status cannot exceed 50 characters")]
    public string? DrugStatus { get; set; }

    [MaxLength(50, ErrorMessage = "Marketing status cannot exceed 50 characters")]
    public string? MarketingStatus { get; set; }

    [MaxLength(50, ErrorMessage = "Legal status cannot exceed 50 characters")]
    public string? LegalStatus { get; set; }

    [MaxLength(50, ErrorMessage = "Domain ID cannot exceed 50 characters")]
    public string? DomainId { get; set; }

    [MaxLength(200, ErrorMessage = "Manufacturer cannot exceed 200 characters")]
    public string? Manufacturer { get; set; }

    [MaxLength(100, ErrorMessage = "Country of origin cannot exceed 100 characters")]
    public string? CountryOfOrigin { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Min stock level must be a positive value")]
    public decimal? MinStockLevel { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Max stock level must be a positive value")]
    public decimal? MaxStockLevel { get; set; }
    
    public int? Status { get; set; }
}
