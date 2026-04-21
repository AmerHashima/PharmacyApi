using MediatR;
using Pharmacy.Application.DTOs.Customer;

namespace Pharmacy.Application.Queries.Customer;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;
