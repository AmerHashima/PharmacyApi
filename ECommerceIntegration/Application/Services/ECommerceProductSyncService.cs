using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceIntegration.Application.Services;

public class ECommerceProductSyncService : IECommerceProductSyncService
{
    private const int DefaultBatchSize = 100;
    private const int MaxBatchSize = 500;

    private readonly IECommerceIntegrationDbContext _db;
    private readonly IECommerceProviderFactory _providerFactory;
    private readonly ILocalProductMatcher _localProductMatcher;
    private readonly ILocalProductCatalogReader _localProductCatalogReader;

    public ECommerceProductSyncService(
        IECommerceIntegrationDbContext db,
        IECommerceProviderFactory providerFactory,
        ILocalProductMatcher localProductMatcher,
        ILocalProductCatalogReader localProductCatalogReader)
    {
        _db = db;
        _providerFactory = providerFactory;
        _localProductMatcher = localProductMatcher;
        _localProductCatalogReader = localProductCatalogReader;
    }

    public async Task<ProductPullResultDto> PullProductsAsync(
        Guid storeId,
        CancellationToken cancellationToken)
    {
        var store = await GetConnectedStoreAsync(storeId, cancellationToken);
        var provider = _providerFactory.GetProvider(store.ProviderType);
        var products = await provider.GetProductsAsync(store.AccessToken!, cancellationToken);
        var result = new ProductPullResultDto { TotalPulled = products.Count };

        foreach (var product in products)
        {
            if (string.IsNullOrWhiteSpace(product.ExternalProductId))
            {
                result.Errors.Add($"Provider product '{product.ProductName ?? product.Sku ?? product.Barcode ?? "unknown"}' has no external id.");
                continue;
            }

            var mapping = await _db.ECommerceProductMappings.FirstOrDefaultAsync(
                x => x.StoreId == store.Oid
                     && x.ExternalProductId == product.ExternalProductId
                     && x.ExternalVariantId == product.ExternalVariantId
                     && !x.IsDeleted,
                cancellationToken);

            if (mapping == null)
            {
                mapping = new ECommerceProductMapping
                {
                    Oid = Guid.NewGuid(),
                    StoreId = store.Oid,
                    ExternalProductId = product.ExternalProductId,
                    ExternalVariantId = product.ExternalVariantId,
                    IsActive = true
                };
                _db.ECommerceProductMappings.Add(mapping);
                result.CreatedMappings++;
            }
            else
            {
                result.UpdatedMappings++;
            }

            mapping.ExternalProductId = product.ExternalProductId;
            mapping.ExternalVariantId = product.ExternalVariantId;
            mapping.ExternalSku = product.Sku;
            mapping.ExternalBarcode = product.Gtin ?? product.Barcode;
            mapping.ExternalProductName = product.ProductName;

            var localMatch = await _localProductMatcher.MatchAsync(product, cancellationToken);
            if (localMatch == null)
            {
                mapping.LocalProductId = null;
                mapping.LocalProductUnitId = null;
                mapping.LocalBarcode = null;
                mapping.IsMapped = false;
                result.UnmatchedCount++;
                continue;
            }

            mapping.LocalProductId = localMatch.LocalProductId;
            mapping.LocalProductUnitId = localMatch.LocalProductUnitId;
            mapping.LocalBarcode = localMatch.LocalBarcode;
            mapping.IsMapped = true;
            result.MatchedCount++;
        }

        await _db.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<ProductPushResultDto> PushInitialProductsAsync(
        Guid storeId,
        Guid? branchId,
        Guid? warehouseId,
        Guid? priceListId,
        int batchSize,
        CancellationToken cancellationToken)
    {
        var store = await GetConnectedStoreAsync(storeId, cancellationToken);
        var provider = _providerFactory.GetProvider(store.ProviderType);
        var result = new ProductPushResultDto();

        var resolvedBranchId = branchId ?? store.BranchId;
        var resolvedWarehouseId = warehouseId ?? store.WarehouseId;
        var resolvedPriceListId = priceListId ?? store.PriceListId;
        var take = NormalizeBatchSize(batchSize);
        var skip = 0;

        while (true)
        {
            var products = await _localProductCatalogReader.GetInitialProductsAsync(
                resolvedBranchId,
                resolvedWarehouseId,
                resolvedPriceListId,
                take,
                skip,
                cancellationToken);

            if (products.Count == 0)
            {
                break;
            }

            result.TotalLocalProducts += products.Count;

            foreach (var product in products.Where(x => x.IsActive))
            {
                var hasMapping = await _db.ECommerceProductMappings.AnyAsync(
                    x => x.StoreId == store.Oid
                         && x.LocalProductId == product.ProductId
                         && x.LocalProductUnitId == product.ProductUnitId
                         && x.IsMapped
                         && x.ExternalProductId != null
                         && !x.IsDeleted,
                    cancellationToken);

                if (hasMapping)
                {
                    result.SkippedAlreadyMappedCount++;
                    continue;
                }

                ExternalProductPushResultDto pushResult;
                try
                {
                    pushResult = await provider.CreateProductAsync(store.AccessToken!, product, cancellationToken);
                }
                catch (Exception ex) when (ex is NotImplementedException or InvalidOperationException)
                {
                    result.FailedCount++;
                    result.Errors.Add($"{product.ProductId} '{product.ProductNameAr}': {ex.Message}");
                    continue;
                }

                if (!pushResult.Success || string.IsNullOrWhiteSpace(pushResult.ExternalProductId))
                {
                    result.FailedCount++;
                    result.Errors.Add($"{product.ProductId} '{product.ProductNameAr}': {pushResult.ErrorMessage ?? "Provider did not return an external product id."}");
                    continue;
                }

                var mapping = new ECommerceProductMapping
                {
                    Oid = Guid.NewGuid(),
                    StoreId = store.Oid,
                    ExternalProductId = pushResult.ExternalProductId,
                    ExternalVariantId = pushResult.ExternalVariantId,
                    ExternalSku = pushResult.ExternalSku ?? product.SKU,
                    ExternalBarcode = pushResult.ExternalBarcode ?? product.GTIN ?? product.Barcode,
                    ExternalProductName = product.ProductNameEn ?? product.ProductNameAr,
                    LocalProductId = product.ProductId,
                    LocalProductUnitId = product.ProductUnitId,
                    LocalBarcode = product.GTIN ?? product.Barcode,
                    IsMapped = true,
                    IsActive = true
                };

                _db.ECommerceProductMappings.Add(mapping);
                result.PushedCount++;
            }

            await _db.SaveChangesAsync(cancellationToken);
            skip += products.Count;

            if (products.Count < take)
            {
                break;
            }
        }

        return result;
    }

    private async Task<ECommerceStore> GetConnectedStoreAsync(Guid storeId, CancellationToken cancellationToken)
    {
        var store = await _db.ECommerceStores.FirstOrDefaultAsync(
            x => x.Oid == storeId && !x.IsDeleted && x.IsActive,
            cancellationToken);

        if (store == null)
        {
            throw new InvalidOperationException("E-Commerce store not found.");
        }

        if (string.IsNullOrWhiteSpace(store.AccessToken))
        {
            throw new InvalidOperationException("E-Commerce store is not connected.");
        }

        return store;
    }

    private static int NormalizeBatchSize(int batchSize)
    {
        if (batchSize <= 0)
        {
            return DefaultBatchSize;
        }

        return Math.Min(batchSize, MaxBatchSize);
    }
}
