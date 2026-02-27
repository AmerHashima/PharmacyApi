using MediatR;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to delete a stock transaction (soft delete)
/// </summary>
public record DeleteStockTransactionCommand(Guid Id) : IRequest<bool>;
