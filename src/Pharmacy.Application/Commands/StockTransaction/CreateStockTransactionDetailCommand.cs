using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to create a new stock transaction detail
/// </summary>
public record CreateStockTransactionDetailCommand(CreateStockTransactionDetailDto Detail) : IRequest<StockTransactionDetailDto>;
