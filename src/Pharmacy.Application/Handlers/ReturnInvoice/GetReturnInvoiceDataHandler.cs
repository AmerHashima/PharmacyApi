using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.ReturnInvoice;
using Pharmacy.Application.Queries.ReturnInvoice;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.ReturnInvoice;

/// <summary>
/// Handler for getting return invoices with advanced filtering, sorting, and pagination
/// </summary>
public class GetReturnInvoiceDataHandler : IRequestHandler<GetReturnInvoiceDataQuery, PagedResult<ReturnInvoiceDto>>
{
    private readonly IReturnInvoiceRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetReturnInvoiceDataHandler(
        IReturnInvoiceRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<ReturnInvoiceDto>> Handle(GetReturnInvoiceDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Apply pagination and get result
        var pagedResult = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);

        var returnInvoiceDtos = _mapper.Map<List<ReturnInvoiceDto>>(pagedResult.Data);

        return PagedResult<ReturnInvoiceDto>.Create(
            returnInvoiceDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
