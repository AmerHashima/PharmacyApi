using MediatR;
using Microsoft.Extensions.Logging;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class SyncDrugListHandler : IRequestHandler<SyncDrugListCommand, DrugListSyncResponseDto>
{
    private readonly IRsdIntegrationService _rsdService;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<SyncDrugListHandler> _logger;

    public SyncDrugListHandler(
        IRsdIntegrationService rsdService,
        IProductRepository productRepository,
        ILogger<SyncDrugListHandler> logger)
    {
        _rsdService = rsdService;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<DrugListSyncResponseDto> Handle(SyncDrugListCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch drug list from RSD
        List<DrugListItemDto> drugs;
        try
        {
            drugs = await _rsdService.GetDrugListAsync(request.Request, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return new DrugListSyncResponseDto
            {
                Success = false,
                ResponseCode = "RSD_ERROR",
                ResponseMessage = ex.Message
            };
        }

        if (drugs.Count == 0)
        {
            return new DrugListSyncResponseDto
            {
                Success = true,
                ResponseCode = "NO_DATA",
                ResponseMessage = "RSD returned no drug records.",
                TotalFromRsd = 0
            };
        }

        // 2. For each drug, find the matching local product and update it
        var updatedProducts = new List<DrugListSyncResultItemDto>();
        var notFoundDrugs  = new List<DrugListItemDto>();
        int skippedCount   = 0;

        foreach (var drug in drugs)
        {
            bool hasGtin = !string.IsNullOrWhiteSpace(drug.GTIN);
            bool hasRegNum = !string.IsNullOrWhiteSpace(drug.RegistrationNumber);

            if (!hasGtin && !hasRegNum)
            {
                skippedCount++;
                continue;
            }

            // Try to match by GTIN first, then RegistrationNumber
            Domain.Entities.Product? product = null;
            string matchedBy = string.Empty;

            if (hasGtin)
            {
                product = await _productRepository.GetByGTINAsync(drug.GTIN!, cancellationToken);
                if (product != null) matchedBy = "GTIN";
            }

            if (product == null && hasRegNum)
            {
                product = await _productRepository.GetByRegistrationNumberAsync(drug.RegistrationNumber!, cancellationToken);
                if (product != null) matchedBy = "RegistrationNumber";
            }

            if (product == null)
            {
                notFoundDrugs.Add(drug);
                continue;
            }

            // 3. Update product fields from RSD data
            ApplyDrugData(product, drug);
            await _productRepository.UpdateAsync(product, cancellationToken);

            updatedProducts.Add(new DrugListSyncResultItemDto
            {
                ProductId          = product.Oid,
                GTIN               = product.GTIN,
                RegistrationNumber = product.RegistrationNumber,
                DrugName           = product.DrugName,
                MatchedBy          = matchedBy
            });

            _logger.LogInformation("DrugList sync: updated product {ProductId} matched by {MatchedBy}", product.Oid, matchedBy);
        }

        return new DrugListSyncResponseDto
        {
            Success         = true,
            ResponseCode    = "OK",
            ResponseMessage = $"Sync complete. Updated: {updatedProducts.Count}, Not found: {notFoundDrugs.Count}, Skipped: {skippedCount}",
            TotalFromRsd    = drugs.Count,
            UpdatedCount    = updatedProducts.Count,
            NotFoundCount   = notFoundDrugs.Count,
            SkippedCount    = skippedCount,
            UpdatedProducts = updatedProducts,
            NotFoundDrugs   = notFoundDrugs
        };
    }

    private static void ApplyDrugData(Domain.Entities.Product product, DrugListItemDto drug)
    {
        if (!string.IsNullOrWhiteSpace(drug.GTIN))
            product.GTIN = drug.GTIN;

        if (!string.IsNullOrWhiteSpace(drug.RegistrationNumber))
            product.RegistrationNumber = drug.RegistrationNumber;

        if (!string.IsNullOrWhiteSpace(drug.DrugName))
            product.DrugName = drug.DrugName;

        if (!string.IsNullOrWhiteSpace(drug.GenericName))
            product.GenericName = drug.GenericName;

        if (!string.IsNullOrWhiteSpace(drug.StrengthValue))
            product.StrengthValue = drug.StrengthValue;

        if (!string.IsNullOrWhiteSpace(drug.StrengthValueUnit))
            product.StrengthUnit = drug.StrengthValueUnit;

        if (!string.IsNullOrWhiteSpace(drug.PackageSize))
            product.PackageSize = drug.PackageSize;

        if (!string.IsNullOrWhiteSpace(drug.DrugStatus))
            product.DrugStatus = drug.DrugStatus;

        if (!string.IsNullOrWhiteSpace(drug.MarketingStatus))
            product.MarketingStatus = drug.MarketingStatus;

        if (!string.IsNullOrWhiteSpace(drug.LegalStatus))
            product.LegalStatus = drug.LegalStatus;

        if (!string.IsNullOrWhiteSpace(drug.DomainId))
            product.DomainId = drug.DomainId;

        if (decimal.TryParse(drug.Price, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var price))
            product.Price = price;

        if (decimal.TryParse(drug.Volume, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var volume))
            product.Volume = volume;

        if (!string.IsNullOrWhiteSpace(drug.UnitOfVolume))
            product.UnitOfVolume = drug.UnitOfVolume;

        if (drug.IsExportable.HasValue)
            product.IsExportable = drug.IsExportable;

        if (drug.IsImportable.HasValue)
            product.IsImportable = drug.IsImportable;
    }
}
