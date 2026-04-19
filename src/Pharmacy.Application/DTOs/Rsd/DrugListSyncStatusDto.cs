namespace Pharmacy.Application.DTOs.Rsd;

public enum DrugListSyncState
{
    Pending,
    Running,
    Completed,
    Failed
}

/// <summary>
/// Tracks the real-time state of an async DrugList sync job
/// </summary>
public class DrugListSyncStatusDto
{
    public Guid JobId { get; init; }
    public DrugListSyncState State { get; set; } = DrugListSyncState.Pending;
    public DateTime StartedAt { get; init; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }

    /// <summary>Full result — populated when State == Completed</summary>
    public DrugListSyncResponseDto? Result { get; set; }
}
