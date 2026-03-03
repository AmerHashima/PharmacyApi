namespace Pharmacy.Application.DTOs.Rsd;

public class PharmacySaleResponseDto
{
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public string? NotificationId { get; set; }
    public List<PharmacySaleProductResultDto> Products { get; set; } = new();
    public string? RawResponse { get; set; }
}

public class PharmacySaleProductResultDto
{
    public string GTIN { get; set; } = string.Empty;
    public string? BatchNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? SerialNumber { get; set; }
    public string? ResponseCode { get; set; }
}