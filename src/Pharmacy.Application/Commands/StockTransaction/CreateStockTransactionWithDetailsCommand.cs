using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to create a stock transaction with its detail lines
/// </summary>
public record CreateStockTransactionWithDetailsCommand(CreateStockTransactionWithDetailsDto Transaction) 
    : IRequest<StockTransactionWithDetailsDto>;
