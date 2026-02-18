using AutoMapper;
using Pharmacy.Application.Commands.Stakeholder;
using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stakeholder;

/// <summary>
/// Handler for updating an existing Stakeholder
/// </summary>
public class UpdateStakeholderHandler : IRequestHandler<UpdateStakeholderCommand, StakeholderDto>
{
    private readonly IStakeholderRepository _repository;
    private readonly IMapper _mapper;

    public UpdateStakeholderHandler(IStakeholderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StakeholderDto> Handle(UpdateStakeholderCommand request, CancellationToken cancellationToken)
    {
        var existingStakeholder = await _repository.GetByIdAsync(request.Stakeholder.Oid, cancellationToken);
        if (existingStakeholder == null)
        {
            throw new KeyNotFoundException($"Stakeholder with ID '{request.Stakeholder.Oid}' not found");
        }

        // Check if GLN is unique (if provided, excluding current stakeholder)
        if (!string.IsNullOrEmpty(request.Stakeholder.GLN))
        {
            if (!await _repository.IsGLNUniqueAsync(request.Stakeholder.GLN, request.Stakeholder.Oid, cancellationToken))
            {
                throw new InvalidOperationException($"GLN '{request.Stakeholder.GLN}' already exists");
            }
        }

        _mapper.Map(request.Stakeholder, existingStakeholder);
        await _repository.UpdateAsync(existingStakeholder, cancellationToken);
        return _mapper.Map<StakeholderDto>(existingStakeholder);
    }
}
