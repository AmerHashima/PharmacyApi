using Pharmacy.Application.DTOs.Product;
using MediatR;

namespace Pharmacy.Application.Queries.Product;

/// <summary>
/// Query to get a product by ID
/// </summary>
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;
