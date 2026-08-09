namespace ECommerceIntegration.Application.Abstractions;

public interface IECommerceOrderSyncService
{
    Task PullOrdersAsync(
        Guid storeId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken);

    Task PullOrderByExternalIdAsync(
        Guid storeId,
        string externalOrderId,
        CancellationToken cancellationToken);
}
