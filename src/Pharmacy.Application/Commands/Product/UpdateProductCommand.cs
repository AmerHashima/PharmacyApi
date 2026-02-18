using Pharmacy.Application.DTOs.Product;
using MediatR;

namespace Pharmacy.Application.Commands.Product;

/// <summary>
/// Command to update an existing Product
/// </summary>
public record UpdateProductCommand(UpdateProductDto Product) : IRequest<ProductDto>;
