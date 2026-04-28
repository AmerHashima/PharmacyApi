using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a doctor or referral source (doctor, clinic, hospital, etc.)
/// </summary>
[Table("Doctors")]
public class Doctor : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string FullNameAr { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullNameEn { get; set; } = string.Empty;

    /// <summary>FK to AppLookupDetail — medical specialty (e.g. General, Cardiology, Pediatrics).</summary>
    public Guid? SpecialtyId { get; set; }

    [ForeignKey(nameof(SpecialtyId))]
    public virtual AppLookupDetail? Specialty { get; set; }

    /// <summary>Professional license number issued by the health authority.</summary>
    [MaxLength(100)]
    public string? LicenseNumber { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    /// <summary>
    /// FK to AppLookupDetail — referral source type.
    /// 1 = Doctor, 2 = Clinic, 3 = Hospital, 4 = Other
    /// </summary>
    public Guid? ReferralTypeId { get; set; }

    [ForeignKey(nameof(ReferralTypeId))]
    public virtual AppLookupDetail? ReferralType { get; set; }

    /// <summary>FK to AppLookupDetail — identity document type (NIN, Iqama, Passport, etc.).</summary>
    public Guid? IdentityTypeId { get; set; }

    [ForeignKey(nameof(IdentityTypeId))]
    public virtual AppLookupDetail? IdentityType { get; set; }

    /// <summary>Identity / ID card number.</summary>
    [MaxLength(50)]
    public string? IdentityNumber { get; set; }

    /// <summary>Commission percentage on referral sales, if applicable.</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? CommissionPercent { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
