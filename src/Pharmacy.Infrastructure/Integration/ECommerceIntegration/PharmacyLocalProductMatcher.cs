using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Integration.ECommerceIntegration;

public class PharmacyLocalProductMatcher : ILocalProductMatcher
{
    private readonly PharmacyDbContext _db;

    public PharmacyLocalProductMatcher(PharmacyDbContext db)
    {
        _db = db;
    }

    public async Task<LocalProductMatchDto?> MatchAsync(
        ECommerceProductDto product,
        CancellationToken cancellationToken)
    {
        var barcode = product.Gtin ?? product.Barcode;
        if (string.IsNullOrWhiteSpace(barcode))
        {
            return null;
        }

        var localProduct = await _db.Products.FirstOrDefaultAsync(
            x => !x.IsDeleted && (x.GTIN == barcode || x.Barcode == barcode),
            cancellationToken);

        if (localProduct != null)
        {
            return new LocalProductMatchDto
            {
                LocalProductId = localProduct.Oid,
                LocalBarcode = localProduct.GTIN ?? localProduct.Barcode
            };
        }

        var unit = await _db.ProductUnits
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => !x.IsDeleted && x.Barcode == barcode, cancellationToken);

        if (unit?.Product == null || unit.Product.IsDeleted)
        {
            return null;
        }

        return new LocalProductMatchDto
        {
            LocalProductId = unit.Product.Oid,
            LocalProductUnitId = unit.Oid,
            LocalBarcode = unit.Product.GTIN ?? unit.Product.Barcode ?? unit.Barcode
        };
    }

    public async Task<LocalProductMatchDto?> GetByIdAsync(
        Guid localProductId,
        CancellationToken cancellationToken)
    {
        var localProduct = await _db.Products.FirstOrDefaultAsync(
            x => x.Oid == localProductId && !x.IsDeleted,
            cancellationToken);

        if (localProduct == null)
        {
            return null;
        }

        return new LocalProductMatchDto
        {
            LocalProductId = localProduct.Oid,
            LocalBarcode = localProduct.GTIN ?? localProduct.Barcode
        };
    }
}
