using AutoMapper;
using Pharmacy.Application.Commands.AppLookup;
using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.AppLookup;

public class CreateAppLookupMasterHandler : IRequestHandler<CreateAppLookupMasterCommand, AppLookupMasterDto>
{
    private readonly IAppLookupMasterRepository _repository;
    private readonly IMapper _mapper;

    public CreateAppLookupMasterHandler(IAppLookupMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppLookupMasterDto> Handle(CreateAppLookupMasterCommand request, CancellationToken cancellationToken)
    {
        // Check if lookup code already exists
        if (await _repository.LookupCodeExistsAsync(request.LookupMaster.LookupCode, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"Lookup code '{request.LookupMaster.LookupCode}' already exists");
        }

        var lookupMaster = _mapper.Map<Domain.Entities.AppLookupMaster>(request.LookupMaster);
        var createdLookupMaster = await _repository.AddAsync(lookupMaster, cancellationToken);
        
        return _mapper.Map<AppLookupMasterDto>(createdLookupMaster);
    }
}