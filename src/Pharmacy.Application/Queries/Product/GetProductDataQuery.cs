using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.Product;

/// <summary>
/// Query to get products with advanced filtering, sorting, and pagination
/// </summary>
public record GetProductDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<ProductDto>>;
