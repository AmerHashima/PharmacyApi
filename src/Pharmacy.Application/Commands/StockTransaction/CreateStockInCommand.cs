using Pharmacy.Application.DTOs.StockTransaction;
using MediatR;
using Pharmacy.Application.Common.Interfaces;

namespace Pharmacy.Application.Commands.StockTransaction;

/// <summary>
/// Command to create a Stock IN transaction (receiving inventory from supplier)
/// </summary>
public record CreateStockInCommand(CreateStockInDto StockIn) : IRequest<StockTransactionDto>, ITransactionalRequest;
