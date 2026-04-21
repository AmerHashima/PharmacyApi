using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Customer;
using Pharmacy.Application.DTOs.Customer;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Customer;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CreateCustomerHandler(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Customer>(request.Customer);
        entity.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<CustomerDto>(entity);
    }
}
