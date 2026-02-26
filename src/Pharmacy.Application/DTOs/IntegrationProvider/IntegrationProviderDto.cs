namespace Pharmacy.Application.DTOs.IntegrationProvider;

public class IntegrationProviderDto
{
    public Guid Oid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}