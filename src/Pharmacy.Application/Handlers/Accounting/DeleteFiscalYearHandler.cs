using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class DeleteFiscalYearHandler : IRequestHandler<DeleteFiscalYearCommand, bool>
{
    private readonly IFiscalYearRepository _repository;

    public DeleteFiscalYearHandler(IFiscalYearRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteFiscalYearCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
