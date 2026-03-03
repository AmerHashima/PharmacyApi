namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// Request DTO for PharmacySaleService — sells products with serial numbers
/// </summary>
public class PharmacySaleRequestDto
{
    public Guid BranchId { get; set; }
    public string? FromGLN { get; set; }
    public List<PharmacySaleProductItemDto> Products { get; set; } = new();
}

public class PharmacySaleProductItemDto
{
    public string GTIN { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
}