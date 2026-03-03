namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// Request DTO for PharmacySaleCancelService — cancels sold products by batch quantity
/// </summary>
public class PharmacySaleCancelRequestDto
{
    public Guid BranchId { get; set; }
    public string? FromGLN { get; set; }
    public List<PharmacySaleCancelProductItemDto> Products { get; set; } = new();
}

public class PharmacySaleCancelProductItemDto
{
    public string GTIN { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
}