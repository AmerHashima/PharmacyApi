using MediatR;

namespace Pharmacy.Application.Commands.ProductUnit;

public record DeleteProductUnitCommand(Guid Id) : IRequest<bool>;
