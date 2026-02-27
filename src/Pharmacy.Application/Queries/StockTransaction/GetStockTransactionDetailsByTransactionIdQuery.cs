using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Queries.StockTransaction;

/// <summary>
/// Query to get all details for a specific stock transaction
/// </summary>
public record GetStockTransactionDetailsByTransactionIdQuery(Guid TransactionId) : IRequest<IEnumerable<StockTransactionDetailDto>>;
