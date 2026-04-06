using MediatR;

namespace Pharmacy.Application.Commands.Store;

public record DeleteStoreCommand(Guid Id) : IRequest<bool>;
