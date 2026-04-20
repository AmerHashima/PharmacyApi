using MediatR;

namespace Pharmacy.Application.Commands.GenericName;

public record DeleteGenericNameCommand(Guid Id) : IRequest<bool>;
