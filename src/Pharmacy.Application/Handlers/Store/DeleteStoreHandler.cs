using MediatR;
using Pharmacy.Application.Commands.Store;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Store;

public class DeleteStoreHandler : IRequestHandler<DeleteStoreCommand, bool>
{
    private readonly IStoreRepository _storeRepository;

    public DeleteStoreHandler(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<bool> Handle(DeleteStoreCommand request, CancellationToken cancellationToken)
    {
        var existing = await _storeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            return false;

        await _storeRepository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
