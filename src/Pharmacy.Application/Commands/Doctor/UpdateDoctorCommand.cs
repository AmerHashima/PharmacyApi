using MediatR;
using Pharmacy.Application.DTOs.Doctor;

namespace Pharmacy.Application.Commands.Doctor;

public record UpdateDoctorCommand(UpdateDoctorDto Doctor) : IRequest<DoctorDto>;
