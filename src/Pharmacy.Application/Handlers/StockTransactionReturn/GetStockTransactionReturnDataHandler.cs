using AutoMapper;
using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.StockTransactionReturn;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Application.Handlers.StockTransactionReturn;

/// <summary>
/// Handler for getting stock transaction returns with advanced filtering, sorting, and pagination
/// </summary>
public class GetStockTransactionReturnDataHandler : IRequestHandler<GetStockTransactionReturnDataQuery, PagedResult<StockTransactionReturnDto>>
{
    private readonly IStockTransactionReturnRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetStockTransactionReturnDataHandler(
        IStockTransactionReturnRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<StockTransactionReturnDto>> Handle(GetStockTransactionReturnDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(x => x.FromBranch)
            .Include(x => x.ToBranch)
            .Include(x => x.TransactionType)
            .Include(x => x.Supplier)
            .Include(x => x.ReturnInvoice)
            .Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);

        // Apply sorting
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        var dtos = _mapper.Map<List<StockTransactionReturnDto>>(pagedResult.Data);

        return PagedResult<StockTransactionReturnDto>.Create(
            dtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
