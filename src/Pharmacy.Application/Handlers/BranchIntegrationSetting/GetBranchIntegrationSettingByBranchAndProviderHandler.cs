using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;
using Pharmacy.Application.Queries.BranchIntegrationSetting;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.BranchIntegrationSetting;

public class GetBranchIntegrationSettingByBranchAndProviderHandler : IRequestHandler<GetBranchIntegrationSettingByBranchAndProviderQuery, BranchIntegrationSettingDto?>
{
    private readonly IBranchIntegrationSettingRepository _repository;
    private readonly IMapper _mapper;

    public GetBranchIntegrationSettingByBranchAndProviderHandler(IBranchIntegrationSettingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BranchIntegrationSettingDto?> Handle(GetBranchIntegrationSettingByBranchAndProviderQuery request, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByBranchAndProviderAsync(request.BranchId, request.ProviderId);
        return setting == null ? null : _mapper.Map<BranchIntegrationSettingDto>(setting);
    }
}