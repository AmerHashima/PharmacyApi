using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Doctor;

public class UpdateDoctorDto
{
    [Required(ErrorMessage = "Doctor ID is required")]
    public Guid Oid { get; set; }

    [Required(ErrorMessage = "Arabic full name is required")]
    [MaxLength(200)]
    public string FullNameAr { get; set; } = string.Empty;

    [Required(ErrorMessage = "English full name is required")]
    [MaxLength(200)]
    public string FullNameEn { get; set; } = string.Empty;

    public Guid? SpecialtyId { get; set; }

    [MaxLength(100)]
    public string? LicenseNumber { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    public Guid? ReferralTypeId { get; set; }
    public Guid? IdentityTypeId { get; set; }

    [MaxLength(50)]
    public string? IdentityNumber { get; set; }

    [Range(0, 100, ErrorMessage = "Commission percent must be between 0 and 100")]
    public decimal? CommissionPercent { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int? Status { get; set; }
}
