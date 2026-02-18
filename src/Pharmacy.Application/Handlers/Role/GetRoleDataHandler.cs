using AutoMapper;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Role;
using Pharmacy.Application.Queries.Role;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Role;

public class GetRoleDataHandler : IRequestHandler<GetRoleDataQuery, PagedResult<RoleDto>>
{
    private readonly IRoleRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetRoleDataHandler(
        IRoleRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<RoleDto>> Handle(GetRoleDataQuery request, CancellationToken cancellationToken)
    {
        // Start with base query - all non-deleted roles
        var query = _repository.GetQueryable()
            .Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);

        // Apply sorting
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        // Apply pagination and get results
        var pagedEntities = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        // Map to DTOs
        var mappedData = _mapper.Map<IEnumerable<RoleDto>>(pagedEntities.Data);

        // Create paged result with mapped data
        return new PagedResult<RoleDto>
        {
            Data = mappedData,
            TotalRecords = pagedEntities.TotalRecords,
            PageNumber = pagedEntities.PageNumber,
            PageSize = pagedEntities.PageSize,
            TotalPages = pagedEntities.TotalPages,
            HasNextPage = pagedEntities.HasNextPage,
            HasPreviousPage = pagedEntities.HasPreviousPage,
            Metadata = new Dictionary<string, object>
            {
                { "availableFilters", new List<string> { "Name", "Description" } },
                { "availableSortFields", new List<string> { "Name", "CreatedAt", "UpdatedAt" } }
            }
        };
    }
}