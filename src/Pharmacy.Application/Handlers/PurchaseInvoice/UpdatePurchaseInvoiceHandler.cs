using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.PurchaseInvoice;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

public class UpdatePurchaseInvoiceHandler : IRequestHandler<UpdatePurchaseInvoiceCommand, PurchaseInvoiceDto>
{
    private readonly IPurchaseInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePurchaseInvoiceHandler(IPurchaseInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PurchaseInvoiceDto> Handle(UpdatePurchaseInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _repository.GetWithPaymentsAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase invoice '{request.Id}' not found.");

        _mapper.Map(request.Invoice, invoice);
        invoice.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(invoice, cancellationToken);

        var updated = await _repository.GetWithPaymentsAsync(invoice.Oid, cancellationToken);
        return _mapper.Map<PurchaseInvoiceDto>(updated!);
    }
}
