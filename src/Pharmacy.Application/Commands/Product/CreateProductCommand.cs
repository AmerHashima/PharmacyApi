using Pharmacy.Application.DTOs.Product;
using MediatR;

namespace Pharmacy.Application.Commands.Product;

/// <summary>
/// Command to create a new Product
/// </summary>
public record CreateProductCommand(CreateProductDto Product) : IRequest<ProductDto>;
