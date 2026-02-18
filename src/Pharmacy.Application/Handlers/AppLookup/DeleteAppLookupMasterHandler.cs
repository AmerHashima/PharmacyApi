using Pharmacy.Application.Commands.AppLookup;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.AppLookup;

/// <summary>
/// Handler for deleting an AppLookupMaster (soft delete)
/// </summary>
public class DeleteAppLookupMasterHandler : IRequestHandler<DeleteAppLookupMasterCommand, bool>
{
    private readonly IAppLookupMasterRepository _repository;
    private readonly IAppLookupDetailRepository _detailRepository;

    public DeleteAppLookupMasterHandler(
        IAppLookupMasterRepository repository,
        IAppLookupDetailRepository detailRepository)
    {
        _repository = repository;
        _detailRepository = detailRepository;
    }

    public async Task<bool> Handle(DeleteAppLookupMasterCommand request, CancellationToken cancellationToken)
    {
        var master = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (master == null)
        {
            return false;
        }

        // Check if it's a system lookup - system lookups cannot be deleted
        if (master.IsSystem)
        {
            throw new InvalidOperationException("System lookup masters cannot be deleted");
        }

        // Soft delete all associated details first
        var details = await _detailRepository.GetByMasterIdAsync(request.Id, cancellationToken);
        foreach (var detail in details)
        {
            await _detailRepository.DeleteAsync(detail.Oid, cancellationToken);
        }

        // Soft delete the master
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
