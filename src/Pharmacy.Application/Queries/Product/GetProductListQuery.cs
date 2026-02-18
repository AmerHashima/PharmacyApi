using Pharmacy.Application.DTOs.Product;
using MediatR;

namespace Pharmacy.Application.Queries.Product;

/// <summary>
/// Query to get all products with optional type filter
/// </summary>
public record GetProductListQuery(Guid? ProductTypeId = null, string? SearchTerm = null) : IRequest<IEnumerable<ProductDto>>;
