using Pharmacy.Application.Commands.Product;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Product;

/// <summary>
/// Handler for deleting a Product (soft delete)
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _repository;

    public DeleteProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
