using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Application.Queries.GenericName;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.GenericName;

public class GetGenericNameDataHandler : IRequestHandler<GetGenericNameDataQuery, PagedResult<GenericNameDto>>
{
    private readonly IGenericNameRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetGenericNameDataHandler(
        IGenericNameRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<GenericNameDto>> Handle(GetGenericNameDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Where(x => !x.IsDeleted);

        var pagedEntities = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);

        var mappedData = _mapper.Map<IEnumerable<GenericNameDto>>(pagedEntities.Data);

        return new PagedResult<GenericNameDto>
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
                { "availableFilters",   new List<string> { "NameEN", "NameAR" } },
                { "availableSortFields", new List<string> { "NameEN", "NameAR", "CreatedAt" } }
            }
        };
    }
}
