namespace Pharmacy.Application.DTOs.Doctor;

public class DoctorDto
{
    public Guid Oid { get; set; }
    public string FullNameAr { get; set; } = string.Empty;
    public string FullNameEn { get; set; } = string.Empty;
    public Guid? SpecialtyId { get; set; }
    public string? SpecialtyName { get; set; }
    public string? SpecialtyNameAr { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Guid? ReferralTypeId { get; set; }
    public string? ReferralTypeName { get; set; }
    public string? ReferralTypeNameAr { get; set; }
    public Guid? IdentityTypeId { get; set; }
    public string? IdentityTypeName { get; set; }
    public string? IdentityNumber { get; set; }
    public decimal? CommissionPercent { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
