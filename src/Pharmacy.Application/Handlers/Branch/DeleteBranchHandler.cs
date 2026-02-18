using Pharmacy.Application.Commands.Branch;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Branch;

/// <summary>
/// Handler for deleting a Branch (soft delete)
/// </summary>
public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, bool>
{
    private readonly IBranchRepository _repository;

    public DeleteBranchHandler(IBranchRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (branch == null)
        {
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
