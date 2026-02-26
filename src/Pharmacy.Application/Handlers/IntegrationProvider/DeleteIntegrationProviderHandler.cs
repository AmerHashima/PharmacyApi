using MediatR;
using Pharmacy.Application.Commands.IntegrationProvider;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.IntegrationProvider;

public class DeleteIntegrationProviderHandler : IRequestHandler<DeleteIntegrationProviderCommand, bool>
{
    private readonly IIntegrationProviderRepository _repository;
    private readonly IBranchIntegrationSettingRepository _branchSettingRepository;

    public DeleteIntegrationProviderHandler(
        IIntegrationProviderRepository repository,
        IBranchIntegrationSettingRepository branchSettingRepository)
    {
        _repository = repository;
        _branchSettingRepository = branchSettingRepository;
    }

    public async Task<bool> Handle(DeleteIntegrationProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id);
        if (provider == null)
            throw new KeyNotFoundException($"Integration provider with ID '{request.Id}' not found");

        // Check if provider is used in any branch settings
        var branchSettings = await _branchSettingRepository.GetByProviderIdAsync(request.Id);
        if (branchSettings.Any())
            throw new InvalidOperationException($"Cannot delete integration provider because it is being used by {branchSettings.Count()} branch setting(s)");

        await _repository.DeleteAsync(request.Id);
        return true;
    }
}