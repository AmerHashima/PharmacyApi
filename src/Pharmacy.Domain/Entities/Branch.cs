using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a pharmacy branch location.
/// A stakeholder (pharmacy) can have multiple branches.
/// </summary>
[Table("Branches")]
public class Branch : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string BranchCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Global Location Number - unique identifier for physical locations
    /// </summary>
    [MaxLength(20)]
    public string? GLN { get; set; }

    /// <summary>
    /// Commercial Registration Number
    /// </summary>
    [MaxLength(20)]
    public string? CRN { get; set; }

    /// <summary>
    /// VAT/Tax Identification Number
    /// </summary>
    [MaxLength(20)]
    public string? VatNumber { get; set; }

    /// <summary>
    /// Identification type lookup (e.g., National ID, Passport, etc.)
    /// </summary>
    public Guid? IdentifyLookupId { get; set; }

    /// <summary>
    /// Identification number/value
    /// </summary>
    [MaxLength(20)]
    public string? IdentifyValue { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? District { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// Street name for detailed addressing
    /// </summary>
    [MaxLength(500)]
    public string? StreetName { get; set; }

    /// <summary>
    /// Building number
    /// </summary>
    [MaxLength(500)]
    public string? BuildingNumber { get; set; }

    /// <summary>
    /// City subdivision/neighborhood name
    /// </summary>
    [MaxLength(500)]
    public string? CitySubdivisionName { get; set; }

    /// <summary>
    /// City name (may differ from City for standardized naming)
    /// </summary]
    [MaxLength(500)]
    public string? CityName { get; set; }

    /// <summary>
    /// Postal/ZIP code zone
    /// </summary>
    [MaxLength(500)]
    public string? PostalZone { get; set; }

    /// <summary>
    /// Official registration name
    /// </summary>
    [MaxLength(500)]
    public string? RegistrationName { get; set; }

    // Navigation Properties
    [ForeignKey(nameof(IdentifyLookupId))]
    public virtual AppLookupDetail? IdentifyLookup { get; set; }

    public virtual ICollection<SystemUser> Users { get; set; } = new List<SystemUser>();
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public virtual ICollection<StockTransaction> OutgoingTransactions { get; set; } = new List<StockTransaction>();
    public virtual ICollection<StockTransaction> IncomingTransactions { get; set; } = new List<StockTransaction>();
    public virtual ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
}
