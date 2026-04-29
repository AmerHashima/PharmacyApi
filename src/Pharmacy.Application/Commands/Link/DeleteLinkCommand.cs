using MediatR;

namespace Pharmacy.Application.Commands.Link;

public record DeleteLinkCommand(Guid Id) : IRequest<bool>;
