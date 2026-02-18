using Pharmacy.Application.Commands.Stakeholder;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stakeholder;

/// <summary>
/// Handler for deleting a Stakeholder (soft delete)
/// </summary>
public class DeleteStakeholderHandler : IRequestHandler<DeleteStakeholderCommand, bool>
{
    private readonly IStakeholderRepository _repository;

    public DeleteStakeholderHandler(IStakeholderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteStakeholderCommand request, CancellationToken cancellationToken)
    {
        var stakeholder = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (stakeholder == null)
        {
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
