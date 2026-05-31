using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.PurchaseInvoice;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

public class CreatePurchaseInvoiceHandler : IRequestHandler<CreatePurchaseInvoiceCommand, PurchaseInvoiceDto>
{
    private readonly IPurchaseInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public CreatePurchaseInvoiceHandler(IPurchaseInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PurchaseInvoiceDto> Handle(CreatePurchaseInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = _mapper.Map<Domain.Entities.PurchaseInvoice>(request.Invoice);
        invoice.PurchaseInvoiceNumber = await GenerateNumberAsync(cancellationToken);
        invoice.CreatedAt = DateTime.UtcNow;

        var payments = request.Invoice.Payments
            .Select(p =>
            {
                var payment = _mapper.Map<PurchaseInvoicePayment>(p);
                payment.PurchaseInvoiceId = invoice.Oid;
                payment.CreatedAt = DateTime.UtcNow;
                return payment;
            })
            .ToList();

        await _repository.InsertMasterDetailAsync(invoice, payments, cancellationToken);

        var created = await _repository.GetWithPaymentsAsync(invoice.Oid, cancellationToken);
        return _mapper.Map<PurchaseInvoiceDto>(created!);
    }

    private async Task<string> GenerateNumberAsync(CancellationToken ct)
    {
        var count = await _repository.CountAsync(ct);
        return $"PI-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D5}";
    }
}
