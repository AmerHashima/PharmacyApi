using MediatR;
using Pharmacy.Application.Commands.GenericName;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.GenericName;

public class DeleteGenericNameHandler : IRequestHandler<DeleteGenericNameCommand, bool>
{
    private readonly IGenericNameRepository _repository;

    public DeleteGenericNameHandler(IGenericNameRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteGenericNameCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return false;

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
