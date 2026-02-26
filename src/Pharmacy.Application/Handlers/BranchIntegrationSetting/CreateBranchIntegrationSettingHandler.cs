using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.BranchIntegrationSetting;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.BranchIntegrationSetting;

public class CreateBranchIntegrationSettingHandler : IRequestHandler<CreateBranchIntegrationSettingCommand, BranchIntegrationSettingDto>
{
    private readonly IBranchIntegrationSettingRepository _repository;
    private readonly IBranchRepository _branchRepository;
    private readonly IIntegrationProviderRepository _providerRepository;
    private readonly IMapper _mapper;

    public CreateBranchIntegrationSettingHandler(
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

    public async Task<BranchIntegrationSettingDto> Handle(CreateBranchIntegrationSettingCommand request, CancellationToken cancellationToken)
    {
        // Validate branch exists
        var branch = await _branchRepository.GetByIdAsync(request.Dto.BranchId);
        if (branch == null)
            throw new InvalidOperationException($"Branch with ID '{request.Dto.BranchId}' not found");

        // Validate provider exists
        var provider = await _providerRepository.GetByIdAsync(request.Dto.IntegrationProviderId);
        if (provider == null)
            throw new InvalidOperationException($"Integration provider with ID '{request.Dto.IntegrationProviderId}' not found");

        // Check if setting already exists for this branch and provider
        if (await _repository.ExistsAsync(request.Dto.BranchId, request.Dto.IntegrationProviderId))
            throw new InvalidOperationException($"Integration setting already exists for this branch and provider");

        var setting = _mapper.Map<Domain.Entities.BranchIntegrationSetting>(request.Dto);
        var createdSetting = await _repository.AddAsync(setting);
        
        // Reload with navigation properties
        var result = await _repository.GetByBranchAndProviderAsync(createdSetting.BranchId, createdSetting.IntegrationProviderId);
        
        return _mapper.Map<BranchIntegrationSettingDto>(result);
    }
}