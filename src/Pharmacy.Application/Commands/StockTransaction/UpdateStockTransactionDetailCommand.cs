using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to update an existing stock transaction detail
/// </summary>
public record UpdateStockTransactionDetailCommand(UpdateStockTransactionDetailDto Detail) : IRequest<StockTransactionDetailDto>;
