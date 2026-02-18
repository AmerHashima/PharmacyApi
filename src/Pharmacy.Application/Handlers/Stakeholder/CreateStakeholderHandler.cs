using AutoMapper;
using Pharmacy.Application.Commands.Stakeholder;
using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stakeholder;

/// <summary>
/// Handler for creating a new Stakeholder
/// </summary>
public class CreateStakeholderHandler : IRequestHandler<CreateStakeholderCommand, StakeholderDto>
{
    private readonly IStakeholderRepository _repository;
    private readonly IMapper _mapper;

    public CreateStakeholderHandler(IStakeholderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StakeholderDto> Handle(CreateStakeholderCommand request, CancellationToken cancellationToken)
    {
        // Check if GLN is unique (if provided)
        if (!string.IsNullOrEmpty(request.Stakeholder.GLN))
        {
            if (!await _repository.IsGLNUniqueAsync(request.Stakeholder.GLN, cancellationToken: cancellationToken))
            {
                throw new InvalidOperationException($"GLN '{request.Stakeholder.GLN}' already exists");
            }
        }

        var stakeholder = _mapper.Map<Domain.Entities.Stakeholder>(request.Stakeholder);
        var createdStakeholder = await _repository.AddAsync(stakeholder, cancellationToken);
        return _mapper.Map<StakeholderDto>(createdStakeholder);
    }
}
