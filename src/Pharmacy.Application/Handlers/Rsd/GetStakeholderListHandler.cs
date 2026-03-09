using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class GetStakeholderListHandler : IRequestHandler<GetStakeholderListCommand, StakeholderListResponseDto>
{
    /// <summary>
    /// MasterID for STAKEHOLDER_TYPE lookup (11111111-1111-1111-1111-111111111003)
    /// </summary>
    private static readonly Guid StakeholderTypeMasterId = Guid.Parse("11111111-1111-1111-1111-111111111003");

    private readonly IRsdIntegrationService _rsdService;
    private readonly IStakeholderRepository _stakeholderRepository;
    private readonly IAppLookupDetailRepository _lookupDetailRepository;

    public GetStakeholderListHandler(
        IRsdIntegrationService rsdService,
        IStakeholderRepository stakeholderRepository,
        IAppLookupDetailRepository lookupDetailRepository)
    {
        _rsdService = rsdService;
        _stakeholderRepository = stakeholderRepository;
        _lookupDetailRepository = lookupDetailRepository;
    }

    public async Task<StakeholderListResponseDto> Handle(GetStakeholderListCommand request, CancellationToken cancellationToken)
    {
        // 1. Call RSD to get the stakeholder list
        var result = await _rsdService.GetStakeholderListAsync(request.Request, cancellationToken);

        if (!result.Success || result.Stakeholders.Count == 0)
            return result;

        // 2. Resolve the StakeholderTypeId from AppLookupDetail
        //    RSD STAKEHOLDERTYPE (1,2,3...) maps to SortOrder in AppLookupDetail
        //    where MasterID = 11111111-1111-1111-1111-111111111003
        //    SortOrder 1=PHARMACY, 2=SUPPLIER, 3=DISTRIBUTOR, 4=MANUFACTURER, 5=WHOLESALER
        var stakeholderTypeDetails = await _lookupDetailRepository.GetByMasterIdAsync(
            StakeholderTypeMasterId, cancellationToken);

        var stakeholderTypeDetail = stakeholderTypeDetails
            .FirstOrDefault(d => d.SortOrder == request.Request.StakeholderType);

        Guid? stakeholderTypeId = stakeholderTypeDetail?.Oid;

        // 3. For each stakeholder from RSD, check if GLN exists and insert if not
        int insertedCount = 0;
        int skippedCount = 0;

        foreach (var rsdStakeholder in result.Stakeholders)
        {
            var existing = await _stakeholderRepository.GetByGLNAsync(rsdStakeholder.GLN, cancellationToken);

            if (existing != null)
            {
                rsdStakeholder.WasInserted = false;
                rsdStakeholder.StakeholderId = existing.Oid;
                skippedCount++;
            }
            else
            {
                var newStakeholder = new Domain.Entities.Stakeholder
                {
                    GLN = rsdStakeholder.GLN,
                    Name = rsdStakeholder.StakeholderName ?? rsdStakeholder.GLN,
                    StakeholderTypeId = stakeholderTypeId,
                    City = rsdStakeholder.CityName,
                    Address = rsdStakeholder.Address,
                    IsActive = rsdStakeholder.IsActive
                };

                var saved = await _stakeholderRepository.AddAsync(newStakeholder, cancellationToken);
                rsdStakeholder.WasInserted = true;
                rsdStakeholder.StakeholderId = saved.Oid;
                insertedCount++;
            }
        }

        result.InsertedCount = insertedCount;
        result.SkippedCount = skippedCount;
        result.ResponseMessage = $"Found {result.TotalCount} stakeholder(s). Inserted: {insertedCount}, Skipped (already exist): {skippedCount}";

        return result;
    }
}
