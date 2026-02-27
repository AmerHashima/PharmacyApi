using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Queries.StockTransaction;

/// <summary>
/// Query to get stock transaction detail by ID
/// </summary>
public record GetStockTransactionDetailByIdQuery(Guid Id) : IRequest<StockTransactionDetailDto?>;
