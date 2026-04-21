using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a customer. All fields except NameEN are optional.
/// Includes all data required by ZATCA (identity, address, VAT number).
/// A default "Cash Patient" seed row is created for anonymous walk-in sales.
/// </summary>
[Table("Customers")]
public class Customer : BaseEntity
{
    /// <summary>Customer name in English (used as the primary display name).</summary>
    [Required]
    [MaxLength(200)]
    public string NameEN { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? NameAR { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    // ── Identity ─────────────────────────────────────────────────────────

    /// <summary>FK to AppLookupDetail — identity type (NIN, Iqama, Passport, etc.).</summary>
    public Guid? IdentityTypeId { get; set; }

    [ForeignKey(nameof(IdentityTypeId))]
    public virtual AppLookupDetail? IdentityType { get; set; }

    /// <summary>Identity / ID card number.</summary>
    [MaxLength(50)]
    public string? IdentityNumber { get; set; }

    // ── ZATCA / VAT ───────────────────────────────────────────────────────

    /// <summary>VAT registration number — required for B2B ZATCA invoices.</summary>
    [MaxLength(20)]
    public string? VatNumber { get; set; }

    // ── Address (ZATCA address fields) ────────────────────────────────────

    [MaxLength(300)]
    public string? AddressStreet { get; set; }

    [MaxLength(10)]
    public string? AddressBuildingNumber { get; set; }

    [MaxLength(10)]
    public string? AddressAdditionalNumber { get; set; }

    [MaxLength(100)]
    public string? AddressDistrict { get; set; }

    [MaxLength(100)]
    public string? AddressCity { get; set; }

    [MaxLength(10)]
    public string? AddressPostalCode { get; set; }

    /// <summary>ISO 3166-1 alpha-2 country code, e.g. "SA".</summary>
    [MaxLength(10)]
    public string? AddressCountry { get; set; }

    // ── Flags ─────────────────────────────────────────────────────────────

    /// <summary>True for the default "Cash Patient" walk-in record.</summary>
    public bool IsWalkIn { get; set; } = false;

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation
    public virtual ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
}
