using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetJournalEntryMasterHandler : IRequestHandler<GetJournalEntryMasterQuery, PagedResult<JournalEntryMasterDto>>
{
    private readonly IJournalEntryRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetJournalEntryMasterHandler(
        IJournalEntryRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<JournalEntryMasterDto>> Handle(
        GetJournalEntryMasterQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Where(j => !j.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);

        return new PagedResult<JournalEntryMasterDto>
        {
            Data          = _mapper.Map<IEnumerable<JournalEntryMasterDto>>(paged.Data),
            TotalRecords  = paged.TotalRecords,
            PageNumber    = paged.PageNumber,
            PageSize      = paged.PageSize,
            TotalPages    = paged.TotalPages,
            HasNextPage   = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
