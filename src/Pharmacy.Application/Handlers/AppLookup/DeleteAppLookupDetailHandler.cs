using Pharmacy.Application.Commands.AppLookup;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.AppLookup;

/// <summary>
/// Handler for deleting an AppLookupDetail (soft delete)
/// </summary>
public class DeleteAppLookupDetailHandler : IRequestHandler<DeleteAppLookupDetailCommand, bool>
{
    private readonly IAppLookupDetailRepository _repository;

    public DeleteAppLookupDetailHandler(IAppLookupDetailRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteAppLookupDetailCommand request, CancellationToken cancellationToken)
    {
        var detail = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (detail == null)
        {
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
