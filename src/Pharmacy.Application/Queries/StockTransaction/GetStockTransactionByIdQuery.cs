using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Queries.StockTransaction;

/// <summary>
/// Query to get stock transaction by ID with details
/// </summary>
public record GetStockTransactionByIdQuery(Guid Id) : IRequest<StockTransactionWithDetailsDto?>;
