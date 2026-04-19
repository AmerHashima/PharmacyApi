using System.Collections.Concurrent;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Infrastructure.Services;

/// <summary>
/// Thread-safe, in-memory tracker for async DrugList sync jobs.
/// Registered as a singleton so it survives across request scopes.
/// </summary>
public sealed class DrugListSyncTracker : IDrugListSyncTracker
{
    private readonly ConcurrentDictionary<Guid, DrugListSyncStatusDto> _jobs = new();

    public DrugListSyncStatusDto Register(Guid jobId)
    {
        var status = new DrugListSyncStatusDto { JobId = jobId, State = DrugListSyncState.Pending };
        _jobs[jobId] = status;
        return status;
    }

    public void MarkRunning(Guid jobId)
    {
        if (_jobs.TryGetValue(jobId, out var status))
            status.State = DrugListSyncState.Running;
    }

    public void MarkCompleted(Guid jobId, DrugListSyncResponseDto result)
    {
        if (_jobs.TryGetValue(jobId, out var status))
        {
            status.Result      = result;
            status.State       = DrugListSyncState.Completed;
            status.CompletedAt = DateTime.UtcNow;
        }
    }

    public void MarkFailed(Guid jobId, string errorMessage)
    {
        if (_jobs.TryGetValue(jobId, out var status))
        {
            status.ErrorMessage = errorMessage;
            status.State        = DrugListSyncState.Failed;
            status.CompletedAt  = DateTime.UtcNow;
        }
    }

    public DrugListSyncStatusDto? GetStatus(Guid jobId) =>
        _jobs.TryGetValue(jobId, out var status) ? status : null;
}
