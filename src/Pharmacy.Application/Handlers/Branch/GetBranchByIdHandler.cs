using AutoMapper;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Application.Queries.Branch;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Branch;

/// <summary>
/// Handler for getting a branch by ID
/// </summary>
public class GetBranchByIdHandler : IRequestHandler<GetBranchByIdQuery, BranchDto?>
{
    private readonly IBranchRepository _repository;
    private readonly IMapper _mapper;

    public GetBranchByIdHandler(IBranchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BranchDto?> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return branch == null ? null : _mapper.Map<BranchDto>(branch);
    }
}
