using MediatR;
using Pharmacy.Application.DTOs.Customer;

namespace Pharmacy.Application.Commands.Customer;

public record CreateCustomerCommand(CreateCustomerDto Customer) : IRequest<CustomerDto>;
