using MediatR;
using Pharmacy.Application.Commands.RoleLink;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.RoleLink;

public class DeleteRoleLinkHandler : IRequestHandler<DeleteRoleLinkCommand, bool>
{
    private readonly IRoleLinkRepository _repository;

    public DeleteRoleLinkHandler(IRoleLinkRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteRoleLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"RoleLink '{request.Id}' not found.");

        await _repository.DeleteAsync(entity.Oid, cancellationToken);
        return true;
    }
}
