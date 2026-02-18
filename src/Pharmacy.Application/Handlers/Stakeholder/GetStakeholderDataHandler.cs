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
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly IMapper _mapper;

    public GetStakeholderDataHandler(
        IStakeholderRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilderService = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<StakeholderDto>> Handle(GetStakeholderDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilderService.ApplyFilters(query, request.Request.Request.Filters);

        // Apply sorting
        query = _queryBuilderService.ApplySorting(query, request.Request.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilderService.ApplyPaginationAsync(query, request.Request.Request.Pagination);

        var stakeholderDtos = _mapper.Map<List<StakeholderDto>>(pagedResult.Data);

        return PagedResult<StakeholderDto>.Create(
            stakeholderDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
