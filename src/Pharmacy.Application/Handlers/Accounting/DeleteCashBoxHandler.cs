using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class DeleteCashBoxHandler : IRequestHandler<DeleteCashBoxCommand, bool>
{
    private readonly ICashBoxRepository _repository;

    public DeleteCashBoxHandler(ICashBoxRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteCashBoxCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
