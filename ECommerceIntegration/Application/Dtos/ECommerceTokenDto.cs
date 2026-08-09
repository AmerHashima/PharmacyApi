namespace ECommerceIntegration.Application.Dtos;

public class ECommerceTokenDto
{
    public string AccessToken { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? ExternalStoreId { get; set; }
    public string? StoreName { get; set; }
}
