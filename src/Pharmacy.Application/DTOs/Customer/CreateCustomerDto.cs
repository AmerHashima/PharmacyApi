using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Customer;

public class CreateCustomerDto
{
    [Required]
    [MaxLength(200)]
    public string NameEN { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? NameAR { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    public Guid? IdentityTypeId { get; set; }

    [MaxLength(50)]
    public string? IdentityNumber { get; set; }

    [MaxLength(20)]
    public string? VatNumber { get; set; }

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

    [MaxLength(10)]
    public string? AddressCountry { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
