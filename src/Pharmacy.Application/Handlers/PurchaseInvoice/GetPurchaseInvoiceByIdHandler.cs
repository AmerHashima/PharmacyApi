using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Application.Queries.PurchaseInvoice;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

public class GetPurchaseInvoiceByIdHandler : IRequestHandler<GetPurchaseInvoiceByIdQuery, PurchaseInvoiceDto?>
{
    private readonly IPurchaseInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public GetPurchaseInvoiceByIdHandler(IPurchaseInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PurchaseInvoiceDto?> Handle(GetPurchaseInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _repository.GetWithPaymentsAsync(request.Id, cancellationToken);
        return invoice == null ? null : _mapper.Map<PurchaseInvoiceDto>(invoice);
    }
}
