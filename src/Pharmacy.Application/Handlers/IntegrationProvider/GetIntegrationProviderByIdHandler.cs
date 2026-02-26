using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.IntegrationProvider;
using Pharmacy.Application.Queries.IntegrationProvider;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.IntegrationProvider;

public class GetIntegrationProviderByIdHandler : IRequestHandler<GetIntegrationProviderByIdQuery, IntegrationProviderDto?>
{
    private readonly IIntegrationProviderRepository _repository;
    private readonly IMapper _mapper;

    public GetIntegrationProviderByIdHandler(IIntegrationProviderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IntegrationProviderDto?> Handle(GetIntegrationProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var provider = await _repository.GetByIdAsync(request.Id);
        return provider == null ? null : _mapper.Map<IntegrationProviderDto>(provider);
    }
}