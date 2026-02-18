using AutoMapper;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.SalesInvoice;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.SalesInvoice;

/// <summary>
/// Handler for getting sales invoices with advanced filtering, sorting, and pagination
/// </summary>
public class GetSalesInvoiceDataHandler : IRequestHandler<GetSalesInvoiceDataQuery, PagedResult<SalesInvoiceDto>>
{
    private readonly ISalesInvoiceRepository _repository;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly IMapper _mapper;

    public GetSalesInvoiceDataHandler(
        ISalesInvoiceRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilderService = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<SalesInvoiceDto>> Handle(GetSalesInvoiceDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilderService.ApplyFilters(query, request.Request.Request.Filters);

        // Apply sorting
        query = _queryBuilderService.ApplySorting(query, request.Request.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilderService.ApplyPaginationAsync(query, request.Request.Request.Pagination);

        var invoiceDtos = _mapper.Map<List<SalesInvoiceDto>>(pagedResult.Data);

        return PagedResult<SalesInvoiceDto>.Create(
            invoiceDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
