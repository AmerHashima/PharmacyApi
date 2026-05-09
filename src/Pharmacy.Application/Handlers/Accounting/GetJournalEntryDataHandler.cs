using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetJournalEntryDataHandler : IRequestHandler<GetJournalEntryDataQuery, PagedResult<JournalEntryDto>>
{
    private readonly IJournalEntryRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetJournalEntryDataHandler(IJournalEntryRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<JournalEntryDto>> Handle(GetJournalEntryDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(j => j.FiscalYear)
            .Include(j => j.Branch)
            .Where(j => !j.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        return new PagedResult<JournalEntryDto>
        {
            Data = _mapper.Map<IEnumerable<JournalEntryDto>>(paged.Data),
            TotalRecords = paged.TotalRecords,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalPages = paged.TotalPages,
            HasNextPage = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
