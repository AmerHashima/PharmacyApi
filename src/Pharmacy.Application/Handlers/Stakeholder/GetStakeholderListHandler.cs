using AutoMapper;
using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Application.Queries.Stakeholder;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stakeholder;

/// <summary>
/// Handler for getting all stakeholders
/// </summary>
public class GetStakeholderListHandler : IRequestHandler<GetStakeholderListQuery, IEnumerable<StakeholderDto>>
{
    private readonly IStakeholderRepository _repository;
    private readonly IMapper _mapper;

    public GetStakeholderListHandler(IStakeholderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StakeholderDto>> Handle(GetStakeholderListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Stakeholder> stakeholders;
        
        if (request.StakeholderTypeId.HasValue)
        {
            stakeholders = await _repository.GetByTypeAsync(request.StakeholderTypeId.Value, cancellationToken);
        }
        else
        {
            stakeholders = await _repository.GetAllAsync(cancellationToken);
        }
        
        return _mapper.Map<IEnumerable<StakeholderDto>>(stakeholders);
    }
}
