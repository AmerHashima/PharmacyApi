using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Customer;
using Pharmacy.Application.DTOs.Customer;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Customer;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCustomerHandler(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Customer.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID '{request.Customer.Oid}' not found");

        if (entity.IsWalkIn)
            throw new InvalidOperationException("The default Cash Patient record cannot be modified.");

        _mapper.Map(request.Customer, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<CustomerDto>(entity);
    }
}
