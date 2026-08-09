namespace ECommerceIntegration.Application.Abstractions;

public interface ILocalInvoiceCreator
{
    Task<Guid> CreateInvoiceAsync(Guid eCommerceOrderId, CancellationToken cancellationToken);
}
