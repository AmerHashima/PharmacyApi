namespace ECommerceIntegration.Application.Dtos;

public class ProductPullResultDto
{
    public int TotalPulled { get; set; }
    public int CreatedMappings { get; set; }
    public int UpdatedMappings { get; set; }
    public int MatchedCount { get; set; }
    public int UnmatchedCount { get; set; }
    public List<string> Errors { get; set; } = new();
}
