using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Customer;
using Pharmacy.Application.Queries.Customer;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Customer;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public GetCustomerByIdHandler(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetQueryable()
            .Include(c => c.IdentityType)
            .Where(c => c.Oid == request.Id && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<CustomerDto>(entity);
    }
}
