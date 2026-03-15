using Pharmacy.Application.DTOs.StockTransactionReturn;
using MediatR;

namespace Pharmacy.Application.Queries.StockTransactionReturn;

/// <summary>
/// Query to get stock transaction returns by branch within a date range
/// </summary>
public record GetStockTransactionReturnListQuery(
    Guid? BranchId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    Guid? TransactionTypeId = null) : IRequest<IEnumerable<StockTransactionReturnDto>>;
