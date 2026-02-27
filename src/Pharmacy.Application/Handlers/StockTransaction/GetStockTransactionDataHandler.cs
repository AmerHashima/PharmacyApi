using AutoMapper;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.StockTransaction;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for getting stock transactions with advanced filtering, sorting, and pagination
/// </summary>
public class GetStockTransactionDataHandler : IRequestHandler<GetStockTransactionDataQuery, PagedResult<StockTransactionDto>>
{
    private readonly IStockTransactionRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetStockTransactionDataHandler(
        IStockTransactionRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<StockTransactionDto>> Handle(GetStockTransactionDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);

        // Apply sorting
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        var transactionDtos = _mapper.Map<List<StockTransactionDto>>(pagedResult.Data);

        return PagedResult<StockTransactionDto>.Create(
            transactionDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
