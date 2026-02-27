using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Branch;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Branch;

/// <summary>
/// Handler for getting branches with advanced filtering, sorting, and pagination
/// </summary>
public class GetBranchDataHandler : IRequestHandler<GetBranchDataQuery, PagedResult<BranchDto>>
{
    private readonly IBranchRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetBranchDataHandler(
        IBranchRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<BranchDto>> Handle(GetBranchDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Execute unified query with filters, sorting, pagination and optional column selection
        var pagedResult = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);

        // Map to BranchDto - AutoMapper handles both Dictionary<string, object> and Branch entity
        var branchDtos = _mapper.Map<List<BranchDto>>(pagedResult.Data);

        return PagedResult<BranchDto>.Create(
            branchDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
