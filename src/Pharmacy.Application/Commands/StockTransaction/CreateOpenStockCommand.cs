using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;

namespace Pharmacy.Application.Queries.StockTransaction;

/// <summary>
/// Query that resolves a list of Open Stock items by GTIN or Barcode.
/// No data is inserted or updated in the database.
/// </summary>
public record ResolveOpenStockQuery(ResolveOpenStockDto Request) : IRequest<OpenStockResultDto>;
