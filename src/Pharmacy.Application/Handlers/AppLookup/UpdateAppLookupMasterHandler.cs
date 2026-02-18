using AutoMapper;
using Pharmacy.Application.Commands.AppLookup;
using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.AppLookup;

/// <summary>
/// Handler for updating an existing AppLookupMaster
/// </summary>
public class UpdateAppLookupMasterHandler : IRequestHandler<UpdateAppLookupMasterCommand, AppLookupMasterDto>
{
    private readonly IAppLookupMasterRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAppLookupMasterHandler(IAppLookupMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppLookupMasterDto> Handle(UpdateAppLookupMasterCommand request, CancellationToken cancellationToken)
    {
        var existingMaster = await _repository.GetByIdAsync(request.LookupMaster.Oid, cancellationToken);
        if (existingMaster == null)
        {
            throw new KeyNotFoundException($"Lookup master with ID '{request.LookupMaster.Oid}' not found");
        }

        // Check if it's a system lookup - system lookups cannot be modified
        if (existingMaster.IsSystem && !request.LookupMaster.IsSystem)
        {
            throw new InvalidOperationException("System lookup masters cannot be modified");
        }

        // Check if lookup code is unique (excluding current)
        if (await _repository.LookupCodeExistsAsync(request.LookupMaster.LookupCode, request.LookupMaster.Oid, cancellationToken))
        {
            throw new InvalidOperationException($"Lookup code '{request.LookupMaster.LookupCode}' already exists");
        }

        // Update properties
        existingMaster.LookupCode = request.LookupMaster.LookupCode;
        existingMaster.LookupNameAr = request.LookupMaster.LookupNameAr;
        existingMaster.LookupNameEn = request.LookupMaster.LookupNameEn;
        existingMaster.Description = request.LookupMaster.Description;
        existingMaster.IsSystem = request.LookupMaster.IsSystem;

        await _repository.UpdateAsync(existingMaster, cancellationToken);
        return _mapper.Map<AppLookupMasterDto>(existingMaster);
    }
}
