using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.IntegrationProvider;
using Pharmacy.Application.DTOs.IntegrationProvider;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.IntegrationProvider;

public class CreateIntegrationProviderHandler : IRequestHandler<CreateIntegrationProviderCommand, IntegrationProviderDto>
{
    private readonly IIntegrationProviderRepository _repository;
    private readonly IMapper _mapper;

    public CreateIntegrationProviderHandler(IIntegrationProviderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IntegrationProviderDto> Handle(CreateIntegrationProviderCommand request, CancellationToken cancellationToken)
    {
        // Check if provider with same name already exists
        if (await _repository.ExistsAsync(request.Dto.Name))
            throw new InvalidOperationException($"Integration provider with name '{request.Dto.Name}' already exists");

        var provider = _mapper.Map<Domain.Entities.IntegrationProvider>(request.Dto);
        var createdProvider = await _repository.AddAsync(provider);
        
        return _mapper.Map<IntegrationProviderDto>(createdProvider);
    }
}