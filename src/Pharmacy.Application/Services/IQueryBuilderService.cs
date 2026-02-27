using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Services;

public interface IQueryBuilderService
{
    IQueryable<T> ApplyFilters<T>(IQueryable<T> query, List<FilterRequest> filters);
    IQueryable<T> ApplySorting<T>(IQueryable<T> query, List<SortRequest> sorts);
    Task<PagedResult<T>> ApplyPaginationAsync<T>(IQueryable<T> query, PaginationRequest pagination);
    IQueryable<T> SelectColumns<T>(IQueryable<T> query, List<string> columns);
    Task<PagedResult<Dictionary<string, object>>> ApplyPaginationWithColumnsAsync<T>(IQueryable<T> query, PaginationRequest pagination, List<string> columns);

    /// <summary>
    /// Unified method to apply filters, sorting, pagination with optional column selection
    /// Returns either full entities or dictionaries based on whether columns are specified
    /// </summary>
    Task<PagedResult<object>> ExecuteQueryAsync<T>(IQueryable<T> query, DataRequest request);
}