using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class DeleteBankAccountHandler : IRequestHandler<DeleteBankAccountCommand, bool>
{
    private readonly IBankAccountRepository _repository;

    public DeleteBankAccountHandler(IBankAccountRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
