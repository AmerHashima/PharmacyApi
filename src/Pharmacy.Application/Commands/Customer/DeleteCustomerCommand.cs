using MediatR;

namespace Pharmacy.Application.Commands.Customer;

public record DeleteCustomerCommand(Guid Id) : IRequest<bool>;
