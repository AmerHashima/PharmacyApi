using MediatR;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for deleting a stock transaction detail (soft delete)
/// </summary>
public class DeleteStockTransactionDetailHandler : IRequestHandler<DeleteStockTransactionDetailCommand, bool>
{
    private readonly IStockTransactionDetailRepository _repository;

    public DeleteStockTransactionDetailHandler(IStockTransactionDetailRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteStockTransactionDetailCommand request, CancellationToken cancellationToken)
    {
        var detail = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (detail == null)
        {
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
