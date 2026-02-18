using AutoMapper;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.Queries.SalesInvoice;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.SalesInvoice;

/// <summary>
/// Handler for getting a sales invoice by ID with items
/// </summary>
public class GetSalesInvoiceByIdHandler : IRequestHandler<GetSalesInvoiceByIdQuery, SalesInvoiceDto?>
{
    private readonly ISalesInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public GetSalesInvoiceByIdHandler(ISalesInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SalesInvoiceDto?> Handle(GetSalesInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _repository.GetWithItemsAsync(request.Id, cancellationToken);
        return invoice == null ? null : _mapper.Map<SalesInvoiceDto>(invoice);
    }
}
