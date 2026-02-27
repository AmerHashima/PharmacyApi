using MediatR;
using Pharmacy.Application.DTOs.Product;

namespace Pharmacy.Application.Queries.Product;

/// <summary>
/// Query to get product by GTIN
/// </summary>
public record GetProductByGTINQuery(string GTIN) : IRequest<ProductDto?>;
