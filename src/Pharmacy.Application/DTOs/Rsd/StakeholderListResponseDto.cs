namespace Pharmacy.Application.DTOs.Rsd;

public class StakeholderListResponseDto
{
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public int TotalCount { get; set; }
    public int InsertedCount { get; set; }
    public int SkippedCount { get; set; }
    public List<RsdStakeholderDto> Stakeholders { get; set; } = new();
    public string? RawResponse { get; set; }
}

public class RsdStakeholderDto
{
    public string GLN { get; set; } = string.Empty;
    public string? StakeholderName { get; set; }
    public string? CityName { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Whether this stakeholder was inserted into the database (false = already existed)</summary>
    public bool WasInserted { get; set; }

    /// <summary>The stakeholder ID in our database (populated after save)</summary>
    public Guid? StakeholderId { get; set; }
}
