using MediatR;
using Pharmacy.Application.DTOs.ProductUnit;

namespace Pharmacy.Application.Queries.ProductUnit;

public record GetProductUnitByIdQuery(Guid Id) : IRequest<ProductUnitDto?>;
