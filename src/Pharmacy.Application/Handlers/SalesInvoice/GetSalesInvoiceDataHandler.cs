using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.Queries.SalesInvoice;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.SalesInvoice;

/// <summary>
/// Handler for getting sales invoices with advanced filtering, sorting, and pagination
/// </summary>
public class GetSalesInvoiceDataHandler : IRequestHandler<GetSalesInvoiceDataQuery, PagedResult<SalesInvoiceDto>>
{
    private readonly ISalesInvoiceRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetSalesInvoiceDataHandler(
        ISalesInvoiceRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<SalesInvoiceDto>> Handle(GetSalesInvoiceDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

     
        // Apply pagination and get result
        var pagedResult = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);

        var invoiceDtos = _mapper.Map<List<SalesInvoiceDto>>(pagedResult.Data);

        return PagedResult<SalesInvoiceDto>.Create(
            invoiceDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
