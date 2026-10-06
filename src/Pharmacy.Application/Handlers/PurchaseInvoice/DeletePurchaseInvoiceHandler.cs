using MediatR;
using Pharmacy.Application.Commands.PurchaseInvoice;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

public class DeletePurchaseInvoiceHandler : IRequestHandler<DeletePurchaseInvoiceCommand, bool>
{
    private readonly IPurchaseInvoiceRepository _repository;

    public DeletePurchaseInvoiceHandler(IPurchaseInvoiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeletePurchaseInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase invoice '{request.Id}' not found.");

        if (invoice.StockTransactionId.HasValue || invoice.JournalEntryId.HasValue)
            throw new InvalidOperationException(
                "A received or posted purchase invoice cannot be deleted. Create a purchase return instead.");

        await _repository.DeleteAsync(invoice.Oid, cancellationToken);
        return true;
    }
}
