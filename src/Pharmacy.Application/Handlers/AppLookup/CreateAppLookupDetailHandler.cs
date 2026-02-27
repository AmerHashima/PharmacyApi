using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.AppLookup;
using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.AppLookup;

/// <summary>
/// Handler for creating a new lookup detail
/// </summary>
public class CreateAppLookupDetailHandler : IRequestHandler<CreateAppLookupDetailCommand, AppLookupDetailDto>
{
    private readonly IAppLookupDetailRepository _repository;
    private readonly IAppLookupMasterRepository _masterRepository;
    private readonly IMapper _mapper;

    public CreateAppLookupDetailHandler(
        IAppLookupDetailRepository repository,
        IAppLookupMasterRepository masterRepository,
        IMapper mapper)
    {
        _repository = repository;
        _masterRepository = masterRepository;
        _mapper = mapper;
    }

    public async Task<AppLookupDetailDto> Handle(CreateAppLookupDetailCommand request, CancellationToken cancellationToken)
    {
        // Validate that lookup master exists
        var masterExists = await _masterRepository.GetByIdAsync(request.LookupDetail.LookupMasterID, cancellationToken);
        if (masterExists == null)
        {
            throw new KeyNotFoundException($"Lookup master with ID '{request.LookupDetail.LookupMasterID}' not found");
        }

        // Check if value code already exists for this lookup master
        var existingDetail = await _repository.GetByValueCodeAsync(
            request.LookupDetail.LookupMasterID,
            request.LookupDetail.ValueCode,
            cancellationToken);

        if (existingDetail != null)
        {
            throw new InvalidOperationException(
                $"Lookup detail with value code '{request.LookupDetail.ValueCode}' already exists for this lookup master");
        }

        // Map DTO to entity
        var lookupDetail = _mapper.Map<AppLookupDetail>(request.LookupDetail);

        // Set audit fields
        lookupDetail.CreatedAt = DateTime.UtcNow;
        lookupDetail.CreatedBy = null; // TODO: Set from current user context (Guid)

        // Add to repository
        await _repository.AddAsync(lookupDetail, cancellationToken);

        // Map back to DTO and return
        return _mapper.Map<AppLookupDetailDto>(lookupDetail);
    }
}
