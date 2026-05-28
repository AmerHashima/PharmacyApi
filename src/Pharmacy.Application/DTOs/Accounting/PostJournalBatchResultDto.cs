namespace Pharmacy.Application.DTOs.Accounting;

/// <summary>
/// Result for a single item in a batch post-journal operation.
/// </summary>
public class PostJournalItemResultDto
{
    public Guid Id { get; set; }
    public bool Success { get; set; }
    public JournalEntryDto? JournalEntry { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Aggregated result for a batch post-journal operation.
/// </summary>
public class PostJournalBatchResultDto
{
    public int TotalRequested { get; set; }
    public int TotalSucceeded { get; set; }
    public int TotalFailed { get; set; }
    public List<PostJournalItemResultDto> Results { get; set; } = [];
}
