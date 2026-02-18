namespace Pharmacy.Application.DTOs.Branch;

/// <summary>
/// DTO for reading Branch data
/// </summary>
public class BranchDto
{
    public Guid Oid { get; set; }
    public string BranchCode { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string? GLN { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int UserCount { get; set; }
    public int StockCount { get; set; }
}
