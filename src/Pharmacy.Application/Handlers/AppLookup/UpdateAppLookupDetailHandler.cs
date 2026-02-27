using AutoMapper;
using Pharmacy.Application.Commands.AppLookup;
using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.AppLookup;

/// <summary>
/// Handler for updating an existing AppLookupDetail
/// </summary>
public class UpdateAppLookupDetailHandler : IRequestHandler<UpdateAppLookupDetailCommand, AppLookupDetailDto>
{
    private readonly IAppLookupDetailRepository _repository;
    private readonly IAppLookupMasterRepository _masterRepository;
    private readonly IMapper _mapper;

    public UpdateAppLookupDetailHandler(
        IAppLookupDetailRepository repository,
        IAppLookupMasterRepository masterRepository,
        IMapper mapper)
    {
        _repository = repository;
        _masterRepository = masterRepository;
        _mapper = mapper;
    }

    public async Task<AppLookupDetailDto> Handle(UpdateAppLookupDetailCommand request, CancellationToken cancellationToken)
    {
        var existingDetail = await _repository.GetByIdAsync(request.LookupDetail.Oid, cancellationToken);
        if (existingDetail == null)
        {
            throw new KeyNotFoundException($"Lookup detail with ID '{request.LookupDetail.Oid}' not found");
        }

        // Verify master exists
        var master = await _masterRepository.GetByIdAsync(request.LookupDetail.MasterID, cancellationToken);
        if (master == null)
        {
            throw new KeyNotFoundException($"Lookup master with ID '{request.LookupDetail.MasterID}' not found");
        }

        // Check if value code is unique within the master (excluding current)
        if (await _repository.ValueCodeExistsAsync(
            request.LookupDetail.MasterID, 
            request.LookupDetail.ValueCode, 
            request.LookupDetail.Oid, 
            cancellationToken))
        {
            throw new InvalidOperationException($"Value code '{request.LookupDetail.ValueCode}' already exists in this lookup");
        }

        // If setting this as default, unset other defaults
        if (request.LookupDetail.IsDefault)
        {
            var currentDefault = await _repository.GetDefaultValueAsync(request.LookupDetail.MasterID, cancellationToken);
            if (currentDefault != null && currentDefault.Oid != request.LookupDetail.Oid)
            {
                currentDefault.IsDefault = false;
                await _repository.UpdateAsync(currentDefault, cancellationToken);
            }
        }

        // Update properties
        existingDetail.MasterID = request.LookupDetail.MasterID;
        existingDetail.ValueCode = request.LookupDetail.ValueCode;
        existingDetail.ValueNameAr = request.LookupDetail.ValueNameAr;
        existingDetail.ValueNameEn = request.LookupDetail.ValueNameEn;
        existingDetail.SortOrder = request.LookupDetail.SortOrder;
        existingDetail.IsDefault = request.LookupDetail.IsDefault;
        existingDetail.IsActive = request.LookupDetail.IsActive;

        await _repository.UpdateAsync(existingDetail, cancellationToken);
        return _mapper.Map<AppLookupDetailDto>(existingDetail);
    }
}
