using MediatR;
using Pharmacy.Application.Commands.InvoiceShape;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.InvoiceShape;

public class DeleteInvoiceShapeHandler : IRequestHandler<DeleteInvoiceShapeCommand, bool>
{
    private readonly IInvoiceShapeRepository _repository;

    public DeleteInvoiceShapeHandler(IInvoiceShapeRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteInvoiceShapeCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            return false;

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
