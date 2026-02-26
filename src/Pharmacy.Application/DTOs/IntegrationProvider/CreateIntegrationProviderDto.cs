namespace Pharmacy.Application.DTOs.IntegrationProvider;

public class CreateIntegrationProviderDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Status { get; set; }
}