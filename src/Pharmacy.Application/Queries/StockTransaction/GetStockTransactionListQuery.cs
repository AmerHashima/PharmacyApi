using Pharmacy.Application.DTOs.StockTransaction;
using MediatR;

namespace Pharmacy.Application.Queries.StockTransaction;

/// <summary>
/// Query to get stock transactions by branch within a date range
/// </summary>
public record GetStockTransactionListQuery(
    Guid? BranchId = null, 
    DateTime? StartDate = null, 
    DateTime? EndDate = null,
    Guid? TransactionTypeId = null) : IRequest<IEnumerable<StockTransactionDto>>;
