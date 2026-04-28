using MediatR;
using Pharmacy.Application.DTOs.Doctor;

namespace Pharmacy.Application.Commands.Doctor;

public record CreateDoctorCommand(CreateDoctorDto Doctor) : IRequest<DoctorDto>;
