using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to update a stock transaction with its detail lines
/// </summary>
public record UpdateStockTransactionWithDetailsCommand(UpdateStockTransactionWithDetailsDto Transaction) 
    : IRequest<StockTransactionWithDetailsDto>;
