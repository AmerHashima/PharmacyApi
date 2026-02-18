using Pharmacy.Application.Commands.SystemUserSpace;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.SystemUserSpace;

public class DeleteSystemUserHandler : IRequestHandler<DeleteSystemUserCommand, bool>
{
    private readonly ISystemUserRepository _repository;

    public DeleteSystemUserHandler(ISystemUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteSystemUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingUser == null)
        {
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}