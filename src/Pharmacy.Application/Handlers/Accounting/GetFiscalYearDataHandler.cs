using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetFiscalYearDataHandler : IRequestHandler<GetFiscalYearDataQuery, PagedResult<FiscalYearDto>>
{
    private readonly IFiscalYearRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetFiscalYearDataHandler(IFiscalYearRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<FiscalYearDto>> Handle(GetFiscalYearDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(f => !f.IsDeleted);
        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        return new PagedResult<FiscalYearDto>
        {
            Data = _mapper.Map<IEnumerable<FiscalYearDto>>(paged.Data),
            TotalRecords = paged.TotalRecords,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalPages = paged.TotalPages,
            HasNextPage = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
