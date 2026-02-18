using Pharmacy.Application.DTOs.StockTransaction;
using MediatR;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to create a Stock Transfer transaction (between branches)
/// </summary>
public record CreateStockTransferCommand(CreateStockTransferDto Transfer) : IRequest<StockTransactionDto>;
