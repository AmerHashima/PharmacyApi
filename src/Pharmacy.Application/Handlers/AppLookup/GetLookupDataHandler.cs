using AutoMapper;
using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.AppLookup;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.AppLookup;

public class GetLookupDataHandler : IRequestHandler<GetLookupDataQuery, PagedResult<AppLookupMasterDto>>
{
    private readonly IAppLookupMasterRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetLookupDataHandler(
        IAppLookupMasterRepository repository, 
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<AppLookupMasterDto>> Handle(GetLookupDataQuery request, CancellationToken cancellationToken)
    {
        // Start with base query - all non-deleted lookup masters
        var query = _repository.GetQueryable()
            .Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);

        // Apply sorting
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        // Apply pagination and get results (includes will be handled in the service)
        var pagedEntities = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        // Map to DTOs
        var mappedData = _mapper.Map<IEnumerable<AppLookupMasterDto>>(pagedEntities.Data);

        // Create paged result with mapped data
        return new PagedResult<AppLookupMasterDto>
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
                { "availableFilters", GetAvailableFilters() },
                { "availableSortFields", GetAvailableSortFields() }
            }
        };
    }

    private static List<string> GetAvailableFilters()
    {
        return new List<string>
        {
            "LookupCode",
            "LookupNameAr", 
            "LookupNameEn",
            "Description",
            "IsSystem",
            "CreatedAt",
            "UpdatedAt"
        };
    }

    private static List<string> GetAvailableSortFields()
    {
        return new List<string>
        {
            "LookupCode",
            "LookupNameAr",
            "LookupNameEn", 
            "IsSystem",
            "CreatedAt",
            "UpdatedAt"
        };
    }
}