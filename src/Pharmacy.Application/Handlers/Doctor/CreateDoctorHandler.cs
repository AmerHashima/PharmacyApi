using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Doctor;
using Pharmacy.Application.DTOs.Doctor;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Doctor;

public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, DoctorDto>
{
    private readonly IDoctorRepository _repository;
    private readonly IMapper _mapper;

    public CreateDoctorHandler(IDoctorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DoctorDto> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Doctor>(request.Doctor);
        entity.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<DoctorDto>(entity);
    }
}
