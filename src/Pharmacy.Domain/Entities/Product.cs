using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents pharmaceutical products in the system.
/// Contains drug information, packaging, pricing, and regulatory status.
/// </summary>
[Table("Products")]
public class Product : BaseEntity
{
    /// <summary>
    /// Global Trade Item Number - unique product identifier (barcode)
    /// </summary>
    [MaxLength(14)]
    public string? GTIN { get; set; }

    [Required]
    [MaxLength(500)]
    public string DrugName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? GenericName { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Product type (Tablet, Syrup, Injection, etc.)
    /// </summary>
    public Guid? ProductTypeId { get; set; }

    [ForeignKey(nameof(ProductTypeId))]
    public virtual AppLookupDetail? ProductType { get; set; }

    /// <summary>
    /// Strength value (e.g., "500" for 500mg)
    /// </summary>
    [MaxLength(50)]
    public string? StrengthValue { get; set; }

    /// <summary>
    /// Strength unit (e.g., "mg", "ml", "IU")
    /// </summary>
    [MaxLength(50)]
    public string? StrengthUnit { get; set; }

    /// <summary>
    /// Package type (e.g., "Box", "Bottle", "Blister")
    /// </summary>
    [MaxLength(100)]
    public string? PackageType { get; set; }

    /// <summary>
    /// Package size (e.g., "30 tablets", "100ml")
    /// </summary>
    [MaxLength(50)]
    public string? PackageSize { get; set; }

    /// <summary>
    /// Unit price of the product
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Drug registration number from regulatory authority
    /// </summary>
    [MaxLength(50)]
    public string? RegistrationNumber { get; set; }

    /// <summary>
    /// Volume of liquid products
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Volume { get; set; }

    /// <summary>
    /// Unit of volume measurement (e.g., "ml", "L")
    /// </summary>
    [MaxLength(50)]
    public string? UnitOfVolume { get; set; }

    /// <summary>
    /// Whether the product can be exported
    /// </summary>
    public bool? IsExportable { get; set; }

    /// <summary>
    /// Whether the product can be imported
    /// </summary>
    public bool? IsImportable { get; set; }

    /// <summary>
    /// Drug status (e.g., "Active", "Discontinued", "Pending")
    /// </summary>
    [MaxLength(50)]
    public string? DrugStatus { get; set; }

    /// <summary>
    /// Marketing authorization status
    /// </summary>
    [MaxLength(50)]
    public string? MarketingStatus { get; set; }

    /// <summary>
    /// Legal classification (e.g., "OTC", "Prescription", "Controlled")
    /// </summary>
    [MaxLength(50)]
    public string? LegalStatus { get; set; }

    /// <summary>
    /// Domain identifier for categorization
    /// </summary>
    [MaxLength(50)]
    public string? DomainId { get; set; }

    /// <summary>
    /// Manufacturer name
    /// </summary>
    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Country of origin
    /// </summary>
    [MaxLength(100)]
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// Minimum stock level for reorder alerts
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinStockLevel { get; set; }

    /// <summary>
    /// Maximum stock level
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxStockLevel { get; set; }

    // Navigation Properties
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    public virtual ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();
    public virtual ICollection<ProductBatch> Batches { get; set; } = new List<ProductBatch>();
}
