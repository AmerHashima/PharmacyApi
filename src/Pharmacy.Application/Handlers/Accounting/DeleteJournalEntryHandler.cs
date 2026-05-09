using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class DeleteJournalEntryHandler : IRequestHandler<DeleteJournalEntryCommand, bool>
{
    private readonly IJournalEntryRepository _repository;

    public DeleteJournalEntryHandler(IJournalEntryRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteJournalEntryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
