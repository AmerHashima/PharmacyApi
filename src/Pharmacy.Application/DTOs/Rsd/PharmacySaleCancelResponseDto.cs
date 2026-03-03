namespace Pharmacy.Application.DTOs.Rsd;

public class PharmacySaleCancelResponseDto
{
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public string? NotificationId { get; set; }
    public List<PharmacySaleCancelProductResultDto> Products { get; set; } = new();
    public string? RawResponse { get; set; }
}

public class PharmacySaleCancelProductResultDto
{
    public string GTIN { get; set; } = string.Empty;
    public string? BatchNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public string? ResponseCode { get; set; }
}