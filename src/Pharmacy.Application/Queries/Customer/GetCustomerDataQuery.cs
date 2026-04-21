using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Customer;

namespace Pharmacy.Application.Queries.Customer;

public record GetCustomerDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<CustomerDto>>;
