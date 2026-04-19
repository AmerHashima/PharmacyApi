using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.Queries.StockTransaction;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Resolves a list of Open Stock items by GTIN or Barcode.
/// Read-only — no inserts or updates are made to the database.
/// Returns two separate lists in the same response:
///   FoundItems    — lines that matched a product (with full product details)
///   NotFoundItems — lines whose GTIN/Barcode could not be matched
/// </summary>
public class ResolveOpenStockHandler : IRequestHandler<ResolveOpenStockQuery, OpenStockResultDto>
{
    private readonly IProductRepository _productRepository;

    public ResolveOpenStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<OpenStockResultDto> Handle(ResolveOpenStockQuery request, CancellationToken cancellationToken)
    {
        var items = request.Request.Items;

        var foundItems    = new List<OpenStockResultItemDto>();
        var notFoundItems = new List<OpenStockNotFoundItemDto>();
        decimal totalValue = 0;

        for (int i = 0; i < items.Count; i++)
        {
            var item    = items[i];
            int lineNum = i + 1;

            var product = await _productRepository.GetByGtinOrBarcodeAsync(item.GtinOrBarcode, cancellationToken);

            if (product == null)
            {
                notFoundItems.Add(new OpenStockNotFoundItemDto
                {
                    LineNumber    = lineNum,
                    GtinOrBarcode = item.GtinOrBarcode,
                    Quantity      = item.Quantity,
                    UnitCost      = item.UnitCost,
                    BatchNumber   = item.BatchNumber,
                    ExpiryDate    = item.ExpiryDate,
                    Reason        = $"No product found for GTIN/Barcode '{item.GtinOrBarcode}'"
                });
                continue;
            }

            var lineCost = item.Quantity * item.UnitCost;
            totalValue += lineCost;

            foundItems.Add(new OpenStockResultItemDto
            {
                LineNumber      = lineNum,
                GtinOrBarcode   = item.GtinOrBarcode,
                ProductId       = product.Oid,
                DrugName        = product.DrugName,
                DrugNameAr      = product.DrugNameAr,
                GTIN            = product.GTIN,
                Barcode         = product.Barcode,
                GenericName     = product.GenericName,
                ProductTypeName = product.ProductType?.ValueNameEn,
                Quantity        = item.Quantity,
                UnitCost        = item.UnitCost,
                TotalCost       = lineCost,
                BatchNumber     = item.BatchNumber,
                ExpiryDate      = item.ExpiryDate
            });
        }

        return new OpenStockResultDto
        {
            TotalInputLines = items.Count,
            TotalFound      = foundItems.Count,
            TotalNotFound   = notFoundItems.Count,
            TotalValue      = totalValue,
            FoundItems      = foundItems,
            NotFoundItems   = notFoundItems
        };
    }
}
