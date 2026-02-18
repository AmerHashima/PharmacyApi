using MediatR;

namespace Pharmacy.Application.Commands.Product;

/// <summary>
/// Command to delete a Product (soft delete)
/// </summary>
public record DeleteProductCommand(Guid Id) : IRequest<bool>;
