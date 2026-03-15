using AutoMapper;
using Pharmacy.Application.DTOs.ReturnInvoice;
using Pharmacy.Application.Queries.ReturnInvoice;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.ReturnInvoice;

/// <summary>
/// Handler for getting return invoices list
/// </summary>
public class GetReturnInvoiceListHandler : IRequestHandler<GetReturnInvoiceListQuery, IEnumerable<ReturnInvoiceDto>>
{
    private readonly IReturnInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public GetReturnInvoiceListHandler(IReturnInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReturnInvoiceDto>> Handle(GetReturnInvoiceListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.ReturnInvoice> returnInvoices;

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            returnInvoices = await _repository.GetByDateRangeAsync(
                request.StartDate.Value,
                request.EndDate.Value,
                request.BranchId,
                cancellationToken);
        }
        else if (request.CashierId.HasValue)
        {
            returnInvoices = await _repository.GetByCashierAsync(request.CashierId.Value, cancellationToken);
        }
        else if (request.BranchId.HasValue)
        {
            returnInvoices = await _repository.GetByBranchAsync(request.BranchId.Value, cancellationToken);
        }
        else
        {
            returnInvoices = await _repository.GetAllAsync(cancellationToken);
        }

        return _mapper.Map<IEnumerable<ReturnInvoiceDto>>(returnInvoices);
    }
}
