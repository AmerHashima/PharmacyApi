using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Doctor;
using Pharmacy.Application.Queries.Doctor;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Doctor;

public class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto?>
{
    private readonly IDoctorRepository _repository;
    private readonly IMapper _mapper;

    public GetDoctorByIdHandler(IDoctorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DoctorDto?> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetQueryable()
            .Include(d => d.Specialty)
            .Include(d => d.ReferralType)
            .Include(d => d.IdentityType)
            .Where(d => d.Oid == request.Id && !d.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<DoctorDto>(entity);
    }
}
