using AutoMapper;
using Pharmacy.Application.Commands.Branch;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Branch;

/// <summary>
/// Handler for creating a new Branch
/// </summary>
public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, BranchDto>
{
    private readonly IBranchRepository _repository;
    private readonly IMapper _mapper;

    public CreateBranchHandler(IBranchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BranchDto> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        // Check if branch code already exists
        if (!await _repository.IsBranchCodeUniqueAsync(request.Branch.BranchCode, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"Branch code '{request.Branch.BranchCode}' already exists");
        }

        var branch = _mapper.Map<Domain.Entities.Branch>(request.Branch);
        var createdBranch = await _repository.AddAsync(branch, cancellationToken);
        return _mapper.Map<BranchDto>(createdBranch);
    }
}
