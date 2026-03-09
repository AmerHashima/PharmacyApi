using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Queries.Rsd;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class GetRsdOperationLogDataHandler : IRequestHandler<GetRsdOperationLogDataQuery, PagedResult<RsdOperationLogDto>>
{
    private readonly IRsdOperationLogRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetRsdOperationLogDataHandler(
        IRsdOperationLogRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<RsdOperationLogDto>> Handle(GetRsdOperationLogDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(x => x.OperationType)
            .Include(x => x.Branch)
            .Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);

        // Apply sorting
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        var dtos = _mapper.Map<List<RsdOperationLogDto>>(pagedResult.Data);

        return PagedResult<RsdOperationLogDto>.Create(
            dtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
