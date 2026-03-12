using MediatR;
using Pharmacy.Application.DTOs.ProductUnit;

namespace Pharmacy.Application.Commands.ProductUnit;

public record UpdateProductUnitCommand(UpdateProductUnitDto ProductUnit) : IRequest<ProductUnitDto>;
