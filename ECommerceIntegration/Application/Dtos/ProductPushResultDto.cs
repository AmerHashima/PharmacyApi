namespace ECommerceIntegration.Application.Dtos;

public class ProductPushResultDto
{
    public int TotalLocalProducts { get; set; }
    public int PushedCount { get; set; }
    public int SkippedAlreadyMappedCount { get; set; }
    public int FailedCount { get; set; }
    public List<string> Errors { get; set; } = new();
}
