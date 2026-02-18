using AutoMapper;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.Queries.SalesInvoice;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.SalesInvoice;

/// <summary>
/// Handler for getting sales invoices list
/// </summary>
public class GetSalesInvoiceListHandler : IRequestHandler<GetSalesInvoiceListQuery, IEnumerable<SalesInvoiceDto>>
{
    private readonly ISalesInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public GetSalesInvoiceListHandler(ISalesInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SalesInvoiceDto>> Handle(GetSalesInvoiceListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.SalesInvoice> invoices;
        
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            invoices = await _repository.GetByDateRangeAsync(
                request.StartDate.Value, 
                request.EndDate.Value, 
                request.BranchId, 
                cancellationToken);
        }
        else if (request.CashierId.HasValue)
        {
            invoices = await _repository.GetByCashierAsync(request.CashierId.Value, cancellationToken);
        }
        else if (request.BranchId.HasValue)
        {
            invoices = await _repository.GetByBranchAsync(request.BranchId.Value, cancellationToken);
        }
        else
        {
            invoices = await _repository.GetAllAsync(cancellationToken);
        }
        
        return _mapper.Map<IEnumerable<SalesInvoiceDto>>(invoices);
    }
}
