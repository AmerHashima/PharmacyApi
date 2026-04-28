using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Doctor;
using Pharmacy.Application.DTOs.Doctor;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Doctor;

public class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommand, DoctorDto>
{
    private readonly IDoctorRepository _repository;
    private readonly IMapper _mapper;

    public UpdateDoctorHandler(IDoctorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DoctorDto> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Doctor.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Doctor with ID '{request.Doctor.Oid}' not found");

        _mapper.Map(request.Doctor, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<DoctorDto>(entity);
    }
}
