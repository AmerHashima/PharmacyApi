using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.IntegrationProvider;
using Pharmacy.Application.Queries.IntegrationProvider;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.IntegrationProvider;

public class GetIntegrationProviderListHandler : IRequestHandler<GetIntegrationProviderListQuery, IEnumerable<IntegrationProviderDto>>
{
    private readonly IIntegrationProviderRepository _repository;
    private readonly IMapper _mapper;

    public GetIntegrationProviderListHandler(IIntegrationProviderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<IntegrationProviderDto>> Handle(GetIntegrationProviderListQuery request, CancellationToken cancellationToken)
    {
        var providers = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<IntegrationProviderDto>>(providers);
    }
}