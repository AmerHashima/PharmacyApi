using MediatR;
using Pharmacy.Application.DTOs.ProductUnit;

namespace Pharmacy.Application.Commands.ProductUnit;

public record CreateProductUnitCommand(CreateProductUnitDto ProductUnit) : IRequest<ProductUnitDto>;
