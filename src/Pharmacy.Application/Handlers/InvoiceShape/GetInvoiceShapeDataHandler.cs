using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.InvoiceShape;
using Pharmacy.Application.Queries.InvoiceShape;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.InvoiceShape;

public class GetInvoiceShapeDataHandler : IRequestHandler<GetInvoiceShapeDataQuery, PagedResult<InvoiceShapeDto>>
{
    private readonly IInvoiceShapeRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetInvoiceShapeDataHandler(IInvoiceShapeRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<InvoiceShapeDto>> Handle(GetInvoiceShapeDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(x => x.Branch)
            .Where(x => !x.IsDeleted);

        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        var paged = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        var dtos = _mapper.Map<List<InvoiceShapeDto>>(paged.Data);
        return PagedResult<InvoiceShapeDto>.Create(dtos, paged.TotalRecords, paged.PageNumber, paged.PageSize);
    }
}
