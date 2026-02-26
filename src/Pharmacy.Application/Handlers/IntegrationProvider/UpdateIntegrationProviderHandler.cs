using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.IntegrationProvider;
using Pharmacy.Application.DTOs.IntegrationProvider;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.IntegrationProvider;

public class UpdateIntegrationProviderHandler : IRequestHandler<UpdateIntegrationProviderCommand, IntegrationProviderDto>
{
    private readonly IIntegrationProviderRepository _repository;
    private readonly IMapper _mapper;

    public UpdateIntegrationProviderHandler(IIntegrationProviderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IntegrationProviderDto> Handle(UpdateIntegrationProviderCommand request, CancellationToken cancellationToken)
    {
        var existingProvider = await _repository.GetByIdAsync(request.Dto.Oid);
        if (existingProvider == null)
            throw new KeyNotFoundException($"Integration provider with ID '{request.Dto.Oid}' not found");

        // Check if another provider with the same name exists
        if (await _repository.ExistsAsync(request.Dto.Name, request.Dto.Oid))
            throw new InvalidOperationException($"Integration provider with name '{request.Dto.Name}' already exists");

        _mapper.Map(request.Dto, existingProvider);
        await _repository.UpdateAsync(existingProvider);
        
        return _mapper.Map<IntegrationProviderDto>(existingProvider);
    }
}