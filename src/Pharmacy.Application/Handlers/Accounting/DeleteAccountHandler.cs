using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly IAccountRepository _repository;

    public DeleteAccountHandler(IAccountRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
