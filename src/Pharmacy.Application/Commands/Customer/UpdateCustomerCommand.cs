using MediatR;
using Pharmacy.Application.DTOs.Customer;

namespace Pharmacy.Application.Commands.Customer;

public record UpdateCustomerCommand(UpdateCustomerDto Customer) : IRequest<CustomerDto>;
