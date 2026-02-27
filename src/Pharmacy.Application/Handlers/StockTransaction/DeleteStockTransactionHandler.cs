using MediatR;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for deleting a stock transaction (soft delete)
/// </summary>
public class DeleteStockTransactionHandler : IRequestHandler<DeleteStockTransactionCommand, bool>
{
    private readonly IStockTransactionRepository _repository;
    private readonly IStockTransactionDetailRepository _detailRepository;

    public DeleteStockTransactionHandler(
        IStockTransactionRepository repository,
        IStockTransactionDetailRepository detailRepository)
    {
        _repository = repository;
        _detailRepository = detailRepository;
    }

    public async Task<bool> Handle(DeleteStockTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction == null)
            return false;

        // Delete detail lines first
        await _detailRepository.DeleteByTransactionIdAsync(request.Id, cancellationToken);

        // Delete transaction header
        await _repository.DeleteAsync(request.Id, cancellationToken);

        return true;
    }
}
