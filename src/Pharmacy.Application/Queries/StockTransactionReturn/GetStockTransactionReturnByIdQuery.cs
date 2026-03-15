using MediatR;
using Pharmacy.Application.DTOs.StockTransactionReturn;

namespace Pharmacy.Application.Queries.StockTransactionReturn;

/// <summary>
/// Query to get stock transaction return by ID with details
/// </summary>
public record GetStockTransactionReturnByIdQuery(Guid Id) : IRequest<StockTransactionReturnWithDetailsDto?>;
