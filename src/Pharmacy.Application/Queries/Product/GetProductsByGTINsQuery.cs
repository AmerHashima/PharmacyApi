using MediatR;
using Pharmacy.Application.DTOs.Product;

namespace Pharmacy.Application.Queries.Product;

/// <summary>
/// Query to get multiple products by list of GTINs
/// </summary>
public record GetProductsByGTINsQuery(IEnumerable<string> GTINs) : IRequest<IEnumerable<ProductDto>>;
