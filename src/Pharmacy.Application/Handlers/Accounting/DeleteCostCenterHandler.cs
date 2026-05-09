using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class DeleteCostCenterHandler : IRequestHandler<DeleteCostCenterCommand, bool>
{
    private readonly ICostCenterRepository _repository;

    public DeleteCostCenterHandler(ICostCenterRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteCostCenterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
