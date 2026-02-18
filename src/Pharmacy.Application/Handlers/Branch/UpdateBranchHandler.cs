using AutoMapper;
using Pharmacy.Application.Commands.Branch;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Branch;

/// <summary>
/// Handler for updating an existing Branch
/// </summary>
public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, BranchDto>
{
    private readonly IBranchRepository _repository;
    private readonly IMapper _mapper;

    public UpdateBranchHandler(IBranchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BranchDto> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var existingBranch = await _repository.GetByIdAsync(request.Branch.Oid, cancellationToken);
        if (existingBranch == null)
        {
            throw new KeyNotFoundException($"Branch with ID '{request.Branch.Oid}' not found");
        }

        // Check if branch code is unique (excluding current branch)
        if (!await _repository.IsBranchCodeUniqueAsync(request.Branch.BranchCode, request.Branch.Oid, cancellationToken))
        {
            throw new InvalidOperationException($"Branch code '{request.Branch.BranchCode}' already exists");
        }

        _mapper.Map(request.Branch, existingBranch);
        await _repository.UpdateAsync(existingBranch, cancellationToken);
        return _mapper.Map<BranchDto>(existingBranch);
    }
}
