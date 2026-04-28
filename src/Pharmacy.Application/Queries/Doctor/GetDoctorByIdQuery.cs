using MediatR;
using Pharmacy.Application.DTOs.Doctor;

namespace Pharmacy.Application.Queries.Doctor;

public record GetDoctorByIdQuery(Guid Id) : IRequest<DoctorDto?>;
