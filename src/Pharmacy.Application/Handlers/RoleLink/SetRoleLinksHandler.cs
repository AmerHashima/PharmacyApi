using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.RoleLink;
using Pharmacy.Application.DTOs.RoleLink;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.RoleLink;

/// <summary>
/// Replaces all RoleLink entries for a role atomically.
/// Existing entries are soft-deleted; new entries are inserted.
/// </summary>
public class SetRoleLinksHandler : IRequestHandler<SetRoleLinksCommand, IEnumerable<RoleLinkDto>>
{
    private readonly IRoleLinkRepository _repository;
    private readonly IMapper _mapper;

    public SetRoleLinksHandler(IRoleLinkRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<RoleLinkDto>> Handle(SetRoleLinksCommand request, CancellationToken cancellationToken)
    {
        var roleId = request.Dto.RoleId;

        // Soft-delete all existing entries for this role
        var existing = await _repository.GetByRoleIdAsync(roleId, cancellationToken);
        foreach (var entry in existing)
        {
            entry.IsDeleted = true;
            entry.DeletedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(entry, cancellationToken);
        }

        // Insert new entries
        foreach (var item in request.Dto.Links)
        {
            var entity = new Domain.Entities.RoleLink
            {
                RoleId    = roleId,
                LinkId    = item.LinkId,
                CanRead   = item.CanRead,
                CanWrite  = item.CanWrite,
                CanEdit   = item.CanEdit,
                CanDelete = item.CanDelete,
                CreatedAt = DateTime.UtcNow,
            };
            await _repository.AddAsync(entity, cancellationToken);
        }

        // Reload with navigation properties
        var reloaded = await _repository.GetByRoleIdAsync(roleId, cancellationToken);
        return _mapper.Map<IEnumerable<RoleLinkDto>>(reloaded);
    }
}
