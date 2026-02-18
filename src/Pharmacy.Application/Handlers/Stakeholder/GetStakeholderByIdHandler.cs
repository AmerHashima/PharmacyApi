using AutoMapper;
using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Application.Queries.Stakeholder;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stakeholder;

/// <summary>
/// Handler for getting a stakeholder by ID
/// </summary>
public class GetStakeholderByIdHandler : IRequestHandler<GetStakeholderByIdQuery, StakeholderDto?>
{
    private readonly IStakeholderRepository _repository;
    private readonly IMapper _mapper;

    public GetStakeholderByIdHandler(IStakeholderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StakeholderDto?> Handle(GetStakeholderByIdQuery request, CancellationToken cancellationToken)
    {
        var stakeholder = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return stakeholder == null ? null : _mapper.Map<StakeholderDto>(stakeholder);
    }
}
