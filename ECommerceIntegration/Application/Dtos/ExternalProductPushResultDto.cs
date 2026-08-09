namespace ECommerceIntegration.Application.Dtos;

public class ExternalProductPushResultDto
{
    public bool Success { get; set; }
    public string? ExternalProductId { get; set; }
    public string? ExternalVariantId { get; set; }
    public string? ExternalSku { get; set; }
    public string? ExternalBarcode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RawResponse { get; set; }
}
