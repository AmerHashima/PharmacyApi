using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetJournalEntryDetailReportHandler : IRequestHandler<GetJournalEntryDetailReportQuery, PagedResult<JournalEntryDetailReportDto>>
{
    private readonly IJournalEntryDetailRepository _repository;
    private readonly IQueryService _queryService;
    private readonly IMapper _mapper;

    public GetJournalEntryDetailReportHandler(IJournalEntryDetailRepository repository, IQueryService queryService, IMapper mapper)
    {
        _repository = repository;
        _queryService = queryService;
        _mapper = mapper;
    }

    public async Task<PagedResult<JournalEntryDetailReportDto>> Handle(GetJournalEntryDetailReportQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(d => d.Account)
            .Include(d => d.CostCenter)
            .Include(d => d.JournalEntry)
                .ThenInclude(j => j.FiscalYear)
            .Include(d => d.JournalEntry)
                .ThenInclude(j => j.Branch)
            .Include(d => d.JournalEntry)
                .ThenInclude(j => j.ReferenceType)
            .Where(d => !d.IsDeleted);

        var paged = await _queryService.ExecuteQueryAsync<JournalEntryDetail>(query, request.QueryRequest.Request);
        return new PagedResult<JournalEntryDetailReportDto>
        {
            Data = _mapper.Map<IEnumerable<JournalEntryDetailReportDto>>(paged.Data),
            TotalRecords = paged.TotalRecords,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalPages = paged.TotalPages,
            HasNextPage = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
