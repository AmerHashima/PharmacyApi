using MediatR;
using Pharmacy.Application.Commands.ProductUnit;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.ProductUnit;

public class DeleteProductUnitHandler : IRequestHandler<DeleteProductUnitCommand, bool>
{
    private readonly IProductUnitRepository _repository;

    public DeleteProductUnitHandler(IProductUnitRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteProductUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
