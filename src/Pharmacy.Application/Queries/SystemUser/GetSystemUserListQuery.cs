using Pharmacy.Application.DTOs.SystemUserSpace;
using MediatR;

namespace Pharmacy.Application.Queries.SystemUserSpace;

public record GetSystemUserListQuery(
    bool IncludeInactive = false,
    int? RoleId = null
) : IRequest<IEnumerable<SystemUserDto>>;