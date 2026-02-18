using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.StockTransaction;

/// <summary>
/// Query to get stock transactions with advanced filtering, sorting, and pagination
/// </summary>
public record GetStockTransactionDataQuery(QueryRequest Request) : IRequest<PagedResult<StockTransactionDto>>;
