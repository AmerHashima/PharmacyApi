using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Services;

public class ECommerceProductMappingService : IECommerceProductMappingService
{
    private readonly IECommerceIntegrationDbContext _db;
    private readonly IECommerceProviderFactory _providerFactory;
    private readonly ILocalProductMatcher _localProductMatcher;

    public ECommerceProductMappingService(
        IECommerceIntegrationDbContext db,
        IECommerceProviderFactory providerFactory,
        ILocalProductMatcher localProductMatcher)
    {
        _db = db;
        _providerFactory = providerFactory;
        _localProductMatcher = localProductMatcher;
    }

    public async Task<IReadOnlyList<ECommerceProductMapping>> ImportProviderProductsAsync(
        Guid storeId,
        CancellationToken cancellationToken)
    {
        var store = await _db.ECommerceStores
            .FirstOrDefaultAsync(x => x.Oid == storeId && !x.IsDeleted && x.IsActive, cancellationToken);

        if (store == null)
        {
            throw new InvalidOperationException("E-Commerce store not found.");
        }

        if (string.IsNullOrWhiteSpace(store.AccessToken))
        {
            throw new InvalidOperationException("E-Commerce store is not connected.");
        }

        var provider = _providerFactory.GetProvider(store.ProviderType);
        var products = await provider.GetProductsAsync(store.AccessToken, cancellationToken);
        var importedMappings = new List<ECommerceProductMapping>();

        foreach (var product in products)
        {
            var mapping = await MatchProductAsync(store.Oid, product, cancellationToken);
            if (mapping != null)
            {
                importedMappings.Add(mapping);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        return importedMappings;
    }

    public async Task<ECommerceProductMapping?> MatchProductAsync(
        Guid storeId,
        ECommerceProductDto product,
        CancellationToken cancellationToken)
    {
        var existing = await _db.ECommerceProductMappings.FirstOrDefaultAsync(
            x => x.StoreId == storeId
                 && x.ExternalProductId == product.ExternalProductId
                 && x.ExternalVariantId == product.ExternalVariantId,
            cancellationToken);

        if (existing != null)
        {
            return existing;
        }

        var localProduct = await _localProductMatcher.MatchAsync(product, cancellationToken);
        var mapping = new ECommerceProductMapping
        {
            Oid = Guid.NewGuid(),
            StoreId = storeId,
            ExternalProductId = product.ExternalProductId,
            ExternalVariantId = product.ExternalVariantId,
            ExternalSku = product.Sku,
            ExternalBarcode = product.Gtin ?? product.Barcode,
            ExternalProductName = product.ProductName,
            LocalProductId = localProduct?.LocalProductId,
            LocalProductUnitId = localProduct?.LocalProductUnitId,
            LocalBarcode = localProduct?.LocalBarcode,
            IsMapped = localProduct != null,
            IsActive = true
        };

        _db.ECommerceProductMappings.Add(mapping);
        return mapping;
    }

    public async Task<ECommerceProductMapping> MapProductAsync(
        Guid mappingId,
        Guid localProductId,
        CancellationToken cancellationToken)
    {
        var mapping = await _db.ECommerceProductMappings
            .FirstOrDefaultAsync(x => x.Oid == mappingId && x.IsActive, cancellationToken);

        if (mapping == null)
        {
            throw new InvalidOperationException("E-Commerce product mapping not found.");
        }

        var localProduct = await _localProductMatcher.GetByIdAsync(localProductId, cancellationToken);

        if (localProduct == null)
        {
            throw new InvalidOperationException("Local product not found.");
        }

        mapping.LocalProductId = localProduct.LocalProductId;
        mapping.LocalProductUnitId = localProduct.LocalProductUnitId;
        mapping.LocalBarcode = localProduct.LocalBarcode;
        mapping.IsMapped = true;

        await _db.SaveChangesAsync(cancellationToken);
        return mapping;
    }

}
