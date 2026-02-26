namespace Pharmacy.Application.DTOs.BranchIntegrationSetting;

public class CreateBranchIntegrationSettingDto
{
    public Guid IntegrationProviderId { get; set; }
    public Guid BranchId { get; set; }
    public string? IntegrationKey { get; set; }
    public string? IntegrationValue { get; set; }
    public int Status { get; set; }
}