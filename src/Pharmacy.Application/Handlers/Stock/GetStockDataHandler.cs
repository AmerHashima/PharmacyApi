using AutoMapper;
using Pharmacy.Application.DTOs.Stock;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Stock;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Application.Handlers.Stock;

/// <summary>
/// Handler for getting stock with advanced filtering, sorting, and pagination
/// </summary>
public class GetStockDataHandler : IRequestHandler<GetStockDataQuery, PagedResult<StockDto>>
{
    private readonly IStockRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetStockDataHandler(
        IStockRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<StockDto>> Handle(GetStockDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Include(x => x.Product)
            .Include(x => x.Branch)
            .Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);

        // Apply sorting
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        var stockDtos = _mapper.Map<List<StockDto>>(pagedResult.Data);

        return PagedResult<StockDto>.Create(
            stockDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
