using Pharmacy.Application.DTOs.Stock;
using MediatR;

namespace Pharmacy.Application.Queries.Stock;

/// <summary>
/// Query to get stock by branch
/// </summary>
public record GetStockByBranchQuery(Guid BranchId) : IRequest<IEnumerable<StockDto>>;
