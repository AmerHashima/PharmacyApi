using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;
using Pharmacy.Application.Queries.BranchIntegrationSetting;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.BranchIntegrationSetting;

public class GetBranchIntegrationSettingsByBranchHandler : IRequestHandler<GetBranchIntegrationSettingsByBranchQuery, IEnumerable<BranchIntegrationSettingDto>>
{
    private readonly IBranchIntegrationSettingRepository _repository;
    private readonly IMapper _mapper;

    public GetBranchIntegrationSettingsByBranchHandler(IBranchIntegrationSettingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BranchIntegrationSettingDto>> Handle(GetBranchIntegrationSettingsByBranchQuery request, CancellationToken cancellationToken)
    {
        var settings = await _repository.GetByBranchIdAsync(request.BranchId);
        return _mapper.Map<IEnumerable<BranchIntegrationSettingDto>>(settings);
    }
}