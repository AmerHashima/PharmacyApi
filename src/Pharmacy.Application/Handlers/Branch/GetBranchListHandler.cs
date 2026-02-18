using AutoMapper;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Application.Queries.Branch;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Branch;

/// <summary>
/// Handler for getting all branches
/// </summary>
public class GetBranchListHandler : IRequestHandler<GetBranchListQuery, IEnumerable<BranchDto>>
{
    private readonly IBranchRepository _repository;
    private readonly IMapper _mapper;

    public GetBranchListHandler(IBranchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BranchDto>> Handle(GetBranchListQuery request, CancellationToken cancellationToken)
    {
        var branches = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BranchDto>>(branches);
    }
}
