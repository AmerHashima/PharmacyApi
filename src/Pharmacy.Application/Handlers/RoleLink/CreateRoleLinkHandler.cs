using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.RoleLink;
using Pharmacy.Application.DTOs.RoleLink;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.RoleLink;

public class CreateRoleLinkHandler : IRequestHandler<CreateRoleLinkCommand, RoleLinkDto>
{
    private readonly IRoleLinkRepository _repository;
    private readonly IMapper _mapper;

    public CreateRoleLinkHandler(IRoleLinkRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<RoleLinkDto> Handle(CreateRoleLinkCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByRoleAndLinkAsync(
            request.Dto.RoleId, request.Dto.LinkId, cancellationToken);

        if (existing != null)
            throw new InvalidOperationException(
                $"A permission entry for Role '{request.Dto.RoleId}' and Link '{request.Dto.LinkId}' already exists. Use Update instead.");

        var entity = _mapper.Map<Domain.Entities.RoleLink>(request.Dto);
        entity.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(entity, cancellationToken);

        // Reload with navigation
        var created = await _repository.GetByRoleAndLinkAsync(entity.RoleId, entity.LinkId, cancellationToken)
                      ?? entity;
        return _mapper.Map<RoleLinkDto>(created);
    }
}
