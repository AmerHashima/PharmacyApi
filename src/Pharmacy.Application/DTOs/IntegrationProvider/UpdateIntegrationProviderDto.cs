namespace Pharmacy.Application.DTOs.IntegrationProvider;

public class UpdateIntegrationProviderDto
{
    public Guid Oid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Status { get; set; }
}