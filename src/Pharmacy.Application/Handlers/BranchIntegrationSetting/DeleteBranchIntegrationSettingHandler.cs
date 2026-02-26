using MediatR;
using Pharmacy.Application.Commands.BranchIntegrationSetting;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.BranchIntegrationSetting;

public class DeleteBranchIntegrationSettingHandler : IRequestHandler<DeleteBranchIntegrationSettingCommand, bool>
{
    private readonly IBranchIntegrationSettingRepository _repository;

    public DeleteBranchIntegrationSettingHandler(IBranchIntegrationSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBranchIntegrationSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByIdAsync(request.Id);
        if (setting == null)
            throw new KeyNotFoundException($"Branch integration setting with ID '{request.Id}' not found");

        await _repository.DeleteAsync(request.Id);
        return true;
    }
}