using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.ProductUnit;

namespace Pharmacy.Application.Queries.ProductUnit;

public record GetProductUnitDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<ProductUnitDto>>;
