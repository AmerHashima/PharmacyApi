using MediatR;
using Pharmacy.Application.DTOs.ProductUnit;

namespace Pharmacy.Application.Queries.ProductUnit;

public record GetProductUnitsByProductIdQuery(Guid ProductId) : IRequest<IEnumerable<ProductUnitDto>>;
