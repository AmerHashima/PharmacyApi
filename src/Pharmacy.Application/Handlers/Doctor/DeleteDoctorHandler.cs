using MediatR;
using Pharmacy.Application.Commands.Doctor;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Doctor;

public class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommand, bool>
{
    private readonly IDoctorRepository _repository;

    public DeleteDoctorHandler(IDoctorRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
