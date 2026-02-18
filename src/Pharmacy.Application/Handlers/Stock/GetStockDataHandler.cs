using AutoMapper;
using Pharmacy.Application.DTOs.Stock;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Stock;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stock;

/// <summary>
/// Handler for getting stock with advanced filtering, sorting, and pagination
/// </summary>
public class GetStockDataHandler : IRequestHandler<GetStockDataQuery, PagedResult<StockDto>>
{
    private readonly IStockRepository _repository;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly IMapper _mapper;

    public GetStockDataHandler(
        IStockRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilderService = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<StockDto>> Handle(GetStockDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilderService.ApplyFilters(query, request.Request.Request.Filters);

        // Apply sorting
        query = _queryBuilderService.ApplySorting(query, request.Request.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilderService.ApplyPaginationAsync(query, request.Request.Request.Pagination);

        var stockDtos = _mapper.Map<List<StockDto>>(pagedResult.Data);

        return PagedResult<StockDto>.Create(
            stockDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
