using MediatR;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to delete a stock transaction detail (soft delete)
/// </summary>
public record DeleteStockTransactionDetailCommand(Guid Id) : IRequest<bool>;
