namespace Pharmacy.Application.DTOs.BranchIntegrationSetting;

public class BranchIntegrationSettingDto
{
    public Guid Oid { get; set; }
    public Guid IntegrationProviderId { get; set; }
    public string IntegrationProviderName { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string? IntegrationKey { get; set; }
    public string? IntegrationValue { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}