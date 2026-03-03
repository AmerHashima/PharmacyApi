namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// Response DTO for the AcceptDispatch SOAP call
/// </summary>
public class AcceptDispatchResponseDto
{
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public string? RawResponse { get; set; }
    public string? DispatchNotificationId { get; set; }
}