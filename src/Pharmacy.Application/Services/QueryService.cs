using Pharmacy.Application.DTOs.Common;
using Microsoft.Extensions.Logging;

namespace Pharmacy.Application.Services;

public interface IQueryService
{
    Task<PagedResult<T>> ExecuteQueryAsync<T>(IQueryable<T> baseQuery, DataRequest request);
}

public class QueryService : IQueryService
{
    private readonly IQueryBuilderService _queryBuilder;
    private readonly ILogger<QueryService> _logger;

    public QueryService(IQueryBuilderService queryBuilder, ILogger<QueryService> logger)
    {
        _queryBuilder = queryBuilder;
        _logger = logger;
    }

    public async Task<PagedResult<T>> ExecuteQueryAsync<T>(IQueryable<T> baseQuery, DataRequest request)
    {
        try
        {
            // Apply filters
            var filteredQuery = _queryBuilder.ApplyFilters(baseQuery, request.Filters);
            
            // Apply sorting
            var sortedQuery = _queryBuilder.ApplySorting(filteredQuery, request.Sort);
            
            // Apply pagination and get results
            return await _queryBuilder.ApplyPaginationAsync(sortedQuery, request.Pagination);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing query for type {EntityType}", typeof(T).Name);
            throw;
        }
    }
}