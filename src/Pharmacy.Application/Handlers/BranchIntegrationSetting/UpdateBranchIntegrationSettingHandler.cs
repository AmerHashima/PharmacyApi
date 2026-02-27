using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.BranchIntegrationSetting;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.BranchIntegrationSetting;

public class UpdateBranchIntegrationSettingHandler : IRequestHandler<UpdateBranchIntegrationSettingCommand, BranchIntegrationSettingDto>
{
    private readonly IBranchIntegrationSettingRepository _repository;
    private readonly IBranchRepository _branchRepository;
    private readonly IIntegrationProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public UpdateBranchIntegrationSettingHandler(
        IBranchIntegrationSettingRepository repository,
        IBranchRepository branchRepository,
        IIntegrationProviderRepository providerRepository,
        IMapper mapper)
    {
        _repository = repository;
        _branchRepository = branchRepository;
        _providerRepository = providerRepository;
        _mapper = mapper;
    }

    public async Task<BranchIntegrationSettingDto> Handle(UpdateBranchIntegrationSettingCommand request, CancellationToken cancellationToken)
    {
        var existingSetting = await _repository.GetByIdAsync(request.Dto.Oid);
        if (existingSetting == null)
            throw new KeyNotFoundException($"Branch integration setting with ID '{request.Dto.Oid}' not found");

        // Validate branch exists
        var branch = await _branchRepository.GetByIdAsync(request.Dto.BranchId);
        if (branch == null)
            throw new InvalidOperationException($"Branch with ID '{request.Dto.BranchId}' not found");

        // Validate provider exists
        var provider = await _providerRepository.GetByIdAsync(request.Dto.IntegrationProviderId);
        if (provider == null)
            throw new InvalidOperationException($"Integration provider with ID '{request.Dto.IntegrationProviderId}' not found");

        // Check if changing to a duplicate branch/provider combination
        //if (existingSetting.BranchId != request.Dto.BranchId || existingSetting.IntegrationProviderId != request.Dto.IntegrationProviderId)
        //{
        //    if (await _repository.ExistsAsync(request.Dto.BranchId, request.Dto.IntegrationProviderId, request.Dto.Oid))
        //        throw new InvalidOperationException($"Integration setting already exists for this branch and provider");
        //}

        _mapper.Map(request.Dto, existingSetting);
        await _repository.UpdateAsync(existingSetting);
        
        // Reload with navigation properties
        var result = await _repository.GetByBranchAndProviderAsync(existingSetting.BranchId, existingSetting.IntegrationProviderId);
        
        return _mapper.Map<BranchIntegrationSettingDto>(result);
    }
}