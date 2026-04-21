using MediatR;
using Pharmacy.Application.Commands.Customer;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Customer;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly ICustomerRepository _repository;

    public DeleteCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        if (entity.IsWalkIn)
            throw new InvalidOperationException("The default Cash Patient record cannot be deleted.");

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
