using MediatR;
using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Application.Common.Interfaces;

namespace Pharmacy.Application.Commands.StockTransactionReturn;

/// <summary>
/// Command to create a stock transaction return with its detail lines
/// </summary>
public record CreateStockTransactionReturnWithDetailsCommand(CreateStockTransactionReturnWithDetailsDto Transaction)
    : IRequest<StockTransactionReturnWithDetailsDto>, ITransactionalRequest;
