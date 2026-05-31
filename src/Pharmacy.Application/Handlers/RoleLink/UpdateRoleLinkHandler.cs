using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.RoleLink;
using Pharmacy.Application.DTOs.RoleLink;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.RoleLink;

public class UpdateRoleLinkHandler : IRequestHandler<UpdateRoleLinkCommand, RoleLinkDto>
{
    private readonly IRoleLinkRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRoleLinkHandler(IRoleLinkRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<RoleLinkDto> Handle(UpdateRoleLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Dto.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"RoleLink '{request.Dto.Oid}' not found.");

        entity.CanRead   = request.Dto.CanRead;
        entity.CanWrite  = request.Dto.CanWrite;
        entity.CanEdit   = request.Dto.CanEdit;
        entity.CanDelete = request.Dto.CanDelete;
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);

        var updated = await _repository.GetByRoleAndLinkAsync(entity.RoleId, entity.LinkId, cancellationToken)
                      ?? entity;
        return _mapper.Map<RoleLinkDto>(updated);
    }
}
