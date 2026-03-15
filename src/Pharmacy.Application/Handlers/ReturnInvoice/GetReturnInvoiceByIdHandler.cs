using AutoMapper;
using Pharmacy.Application.DTOs.ReturnInvoice;
using Pharmacy.Application.Queries.ReturnInvoice;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.ReturnInvoice;

/// <summary>
/// Handler for getting a return invoice by ID with items
/// </summary>
public class GetReturnInvoiceByIdHandler : IRequestHandler<GetReturnInvoiceByIdQuery, ReturnInvoiceDto?>
{
    private readonly IReturnInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public GetReturnInvoiceByIdHandler(IReturnInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ReturnInvoiceDto?> Handle(GetReturnInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var returnInvoice = await _repository.GetWithItemsAsync(request.Id, cancellationToken);
        return returnInvoice == null ? null : _mapper.Map<ReturnInvoiceDto>(returnInvoice);
    }
}
