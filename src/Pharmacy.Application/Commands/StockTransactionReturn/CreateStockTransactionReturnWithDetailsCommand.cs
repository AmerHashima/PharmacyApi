using MediatR;
using Pharmacy.Application.DTOs.StockTransactionReturn;

namespace Pharmacy.Application.Commands.StockTransactionReturn;

/// <summary>
/// Command to create a stock transaction return with its detail lines
/// </summary>
public record CreateStockTransactionReturnWithDetailsCommand(CreateStockTransactionReturnWithDetailsDto Transaction)
    : IRequest<StockTransactionReturnWithDetailsDto>;
