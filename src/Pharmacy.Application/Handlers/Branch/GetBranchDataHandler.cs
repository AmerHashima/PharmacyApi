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
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly IMapper _mapper;

    public GetBranchDataHandler(
        IBranchRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilderService = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<BranchDto>> Handle(GetBranchDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilderService.ApplyFilters(query, request.Request.Request.Filters);

        // Apply sorting
        query = _queryBuilderService.ApplySorting(query, request.Request.Request.Sort);

        // Check if column selection is requested
        var columns = request.Request.Request.Columns;
        if (columns != null && columns.Count > 0)
        {
            // Use column-specific pagination that queries only selected columns from database
            var pagedDictResult = await _queryBuilderService.ApplyPaginationWithColumnsAsync(
                query, 
                request.Request.Request.Pagination,
                columns);

            // Map dictionaries to BranchDto using AutoMapper (only selected properties will have values)
            var branchDtos = _mapper.Map<List<BranchDto>>(pagedDictResult.Data);

            return PagedResult<BranchDto>.Create(
                branchDtos,
                pagedDictResult.TotalRecords,
                pagedDictResult.PageNumber,
                pagedDictResult.PageSize);
        }
        else
        {
            // Apply pagination and get full result with all columns
            var pagedResult = await _queryBuilderService.ApplyPaginationAsync(query, request.Request.Request.Pagination);

            var branchDtos = _mapper.Map<List<BranchDto>>(pagedResult.Data);

            return PagedResult<BranchDto>.Create(
                branchDtos,
                pagedResult.TotalRecords,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}
