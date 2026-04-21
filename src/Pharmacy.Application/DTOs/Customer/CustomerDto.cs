namespace Pharmacy.Application.DTOs.Customer;

public class CustomerDto
{
    public Guid Oid { get; set; }
    public string NameEN { get; set; } = string.Empty;
    public string? NameAR { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? IdentityTypeId { get; set; }
    public string? IdentityTypeName { get; set; }
    public string? IdentityNumber { get; set; }
    public string? VatNumber { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressBuildingNumber { get; set; }
    public string? AddressAdditionalNumber { get; set; }
    public string? AddressDistrict { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressPostalCode { get; set; }
    public string? AddressCountry { get; set; }
    public bool IsWalkIn { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
}
