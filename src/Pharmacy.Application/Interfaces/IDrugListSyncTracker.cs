using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Interfaces;

/// <summary>
/// Singleton service that tracks the status of async DrugList sync jobs.
/// Thread-safe; safe to use from both the request thread and background threads.
/// </summary>
public interface IDrugListSyncTracker
{
    /// <summary>Register a new job and return its initial status.</summary>
    DrugListSyncStatusDto Register(Guid jobId);

    /// <summary>Mark a job as Running.</summary>
    void MarkRunning(Guid jobId);

    /// <summary>Mark a job as Completed and store the result.</summary>
    void MarkCompleted(Guid jobId, DrugListSyncResponseDto result);

    /// <summary>Mark a job as Failed and record the error.</summary>
    void MarkFailed(Guid jobId, string errorMessage);

    /// <summary>Retrieve the current status of a job, or null if unknown.</summary>
    DrugListSyncStatusDto? GetStatus(Guid jobId);
}
