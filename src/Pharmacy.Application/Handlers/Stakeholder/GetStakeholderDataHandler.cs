using AutoMapper;
using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Stakeholder;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stakeholder;

/// <summary>
/// Handler for getting stakeholders with advanced filtering, sorting, and pagination
/// </summary>
public class GetStakeholderDataHandler : IRequestHandler<GetStakeholderDataQuery, PagedResult<StakeholderDto>>
{
    private readonly IStakeholderRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetStakeholderDataHandler(
        IStakeholderRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<StakeholderDto>> Handle(GetStakeholderDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);

        // Apply sorting
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        var stakeholderDtos = _mapper.Map<List<StakeholderDto>>(pagedResult.Data);

        return PagedResult<StakeholderDto>.Create(
            stakeholderDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
