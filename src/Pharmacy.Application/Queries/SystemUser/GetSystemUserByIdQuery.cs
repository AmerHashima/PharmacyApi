using Pharmacy.Application.DTOs.SystemUserSpace;
using MediatR;

namespace Pharmacy.Application.Queries.SystemUserSpace;

public record GetSystemUserByIdQuery(Guid Id) : IRequest<SystemUserDto?>;