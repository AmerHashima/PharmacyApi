using Pharmacy.Application.Commands.Role;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Role;

public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleRepository _repository;

    public DeleteRoleHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (role == null)
        {
            return false;
        }

        // TODO: Check if role is assigned to any users before deleting
        // This would prevent deleting roles that are in use

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}