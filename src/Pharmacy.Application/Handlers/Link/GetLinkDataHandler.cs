using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Link;
using Pharmacy.Application.Queries.Link;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Link;

public class GetLinkDataHandler : IRequestHandler<GetLinkDataQuery, PagedResult<LinkDto>>
{
    private readonly ILinkRepository _linkRepository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetLinkDataHandler(ILinkRepository linkRepository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _linkRepository = linkRepository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<LinkDto>> Handle(GetLinkDataQuery request, CancellationToken cancellationToken)
    {
        var query = _linkRepository.GetQueryable()
            .Include(l => l.ReportParameters.Where(p => !p.IsDeleted))
            .Where(l => !l.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        var mapped = _mapper.Map<IEnumerable<LinkDto>>(paged.Data);

        return new PagedResult<LinkDto>
        {
            Data = mapped,
            TotalRecords = paged.TotalRecords,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalPages = paged.TotalPages,
            HasNextPage = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage,
            Metadata = new Dictionary<string, object>
            {
                { "availableFilters",    new List<string> { "NameAr", "NameEn", "Path", "ReportsKey" } },
                { "availableSortFields", new List<string> { "NameAr", "NameEn", "CreatedAt" } }
            }
        };
    }
}
