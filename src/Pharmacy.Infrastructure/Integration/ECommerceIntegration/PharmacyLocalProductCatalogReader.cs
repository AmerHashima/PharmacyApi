using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Integration.ECommerceIntegration;

public class PharmacyLocalProductCatalogReader : ILocalProductCatalogReader
{
    private readonly PharmacyDbContext _db;

    public PharmacyLocalProductCatalogReader(PharmacyDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<LocalProductCatalogItemDto>> GetInitialProductsAsync(
        Guid? branchId,
        Guid? warehouseId,
        Guid? priceListId,
        int take,
        int skip,
        CancellationToken cancellationToken)
    {
        var products = await _db.Products
            .AsNoTracking()
            .Include(x => x.GenericNameRef)
            .Include(x => x.VatType)
            .Include(x => x.Units)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.DrugName)
            .ThenBy(x => x.Oid)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var quantities = await GetQuantitiesAsync(
            products.Select(x => x.Oid).ToArray(),
            branchId,
            cancellationToken);

        return products
            .SelectMany(product => MapProduct(product, quantities.GetValueOrDefault(product.Oid)))
            .ToList();
    }

    public async Task<LocalProductCatalogItemDto?> GetProductByIdAsync(
        Guid productId,
        Guid? productUnitId,
        CancellationToken cancellationToken)
    {
        var product = await _db.Products
            .AsNoTracking()
            .Include(x => x.GenericNameRef)
            .Include(x => x.VatType)
            .Include(x => x.Units)
            .FirstOrDefaultAsync(x => x.Oid == productId && !x.IsDeleted, cancellationToken);

        if (product == null)
        {
            return null;
        }

        var quantity = await GetQuantityAsync(product.Oid, null, cancellationToken);
        var items = MapProduct(product, quantity);
        return productUnitId.HasValue
            ? items.FirstOrDefault(x => x.ProductUnitId == productUnitId)
            : items.FirstOrDefault(x => x.ProductUnitId == null) ?? items.FirstOrDefault();
    }

    private async Task<Dictionary<Guid, decimal?>> GetQuantitiesAsync(
        IReadOnlyCollection<Guid> productIds,
        Guid? branchId,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return new Dictionary<Guid, decimal?>();
        }

        var query = _db.Stocks
            .AsNoTracking()
            .Where(x => !x.IsDeleted && productIds.Contains(x.ProductId));

        if (branchId.HasValue)
        {
            query = query.Where(x => x.BranchId == branchId.Value);
        }

        return await query
            .GroupBy(x => x.ProductId)
            .Select(x => new
            {
                ProductId = x.Key,
                Quantity = x.Sum(s => (s.Quantity ?? 0) - (s.ReservedQuantity ?? 0))
            })
            .ToDictionaryAsync(x => x.ProductId, x => (decimal?)x.Quantity, cancellationToken);
    }

    private async Task<decimal?> GetQuantityAsync(
        Guid productId,
        Guid? branchId,
        CancellationToken cancellationToken)
    {
        var quantities = await GetQuantitiesAsync(new[] { productId }, branchId, cancellationToken);
        return quantities.GetValueOrDefault(productId);
    }

    private static IReadOnlyList<LocalProductCatalogItemDto> MapProduct(
        Product product,
        decimal? quantity)
    {
        var activeUnits = product.Units
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.ConversionFactor)
            .ThenBy(x => x.Oid)
            .ToList();

        if (activeUnits.Count == 0)
        {
            return new[]
            {
                CreateItem(product, null, quantity)
            };
        }

        return activeUnits
            .Select(unit => CreateItem(product, unit, quantity))
            .ToList();
    }

    private static LocalProductCatalogItemDto CreateItem(
        Product product,
        ProductUnit? unit,
        decimal? quantity)
    {
        var barcode = unit?.Barcode ?? product.Barcode;
        var price = unit?.Price ?? product.Price ?? 0;
        var productNameAr = FirstNonEmpty(product.DrugNameAr, product.DrugName) ?? string.Empty;
        var productNameEn = FirstNonEmpty(product.GenericNameRef?.NameEN, product.GenericName, product.DrugName);
        var isActive = !string.Equals(product.DrugStatus, "DISCONTINUED", StringComparison.OrdinalIgnoreCase);

        return new LocalProductCatalogItemDto
        {
            ProductId = product.Oid,
            ProductUnitId = unit?.Oid,
            ProductCode = product.RegistrationNumber,
            ProductNameAr = productNameAr,
            ProductNameEn = productNameEn,
            DescriptionAr = product.DrugNameAr,
            DescriptionEn = product.GenericNameRef?.NameEN ?? product.GenericName,
            Barcode = barcode,
            GTIN = product.GTIN,
            SKU = FirstNonEmpty(product.GTIN, barcode, product.RegistrationNumber, product.Oid.ToString()),
            Price = price,
            CostPrice = null,
            Quantity = quantity,
            VatPercent = ResolveVatPercent(product.VatType?.ValueCode),
            IsVatIncluded = true,
            IsActive = isActive,
            ImageUrl = null,
            ExtraData = new Dictionary<string, string>
            {
                ["productId"] = product.Oid.ToString(),
                ["productUnitId"] = unit?.Oid.ToString() ?? string.Empty,
                ["packageSize"] = product.PackageSize ?? string.Empty,
                ["manufacturer"] = product.Manufacturer ?? string.Empty,
                ["conversionFactor"] = unit?.ConversionFactor.ToString() ?? string.Empty
            }
        };
    }

    private static decimal? ResolveVatPercent(string? valueCode)
    {
        if (string.IsNullOrWhiteSpace(valueCode))
        {
            return null;
        }

        var digits = new string(valueCode.Where(char.IsDigit).ToArray());
        return decimal.TryParse(digits, out var percent) ? percent : null;
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
    }
}
