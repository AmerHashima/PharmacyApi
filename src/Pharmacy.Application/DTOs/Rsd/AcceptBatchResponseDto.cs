namespace Pharmacy.Application.DTOs.Rsd;

public class AcceptBatchResponseDto
{
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public string? NotificationId { get; set; }
    public List<AcceptBatchProductResultDto> Products { get; set; } = new();
    public string? RawResponse { get; set; }
}

public class AcceptBatchProductResultDto
{
    public string GTIN { get; set; } = string.Empty;
    public string? BatchNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public int Quantity { get; set; }

    /// <summary>Response Code per product (e.g. 10201)</summary>
    public string? ResponseCode { get; set; }
}