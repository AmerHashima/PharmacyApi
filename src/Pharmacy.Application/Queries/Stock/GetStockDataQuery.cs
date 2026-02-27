using Pharmacy.Application.DTOs.Stock;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.Stock;

/// <summary>
/// Query to get stock with advanced filtering, sorting, and pagination
/// </summary>
public record GetStockDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<StockDto>>;
