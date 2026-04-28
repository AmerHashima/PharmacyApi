using MediatR;

namespace Pharmacy.Application.Commands.Doctor;

public record DeleteDoctorCommand(Guid Id) : IRequest<bool>;
