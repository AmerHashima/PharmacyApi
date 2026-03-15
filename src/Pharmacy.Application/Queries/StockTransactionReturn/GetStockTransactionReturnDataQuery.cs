using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.StockTransactionReturn;

/// <summary>
/// Query to get stock transaction returns with advanced filtering, sorting, and pagination
/// </summary>
public record GetStockTransactionReturnDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<StockTransactionReturnDto>>;
