using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetCashBoxDataHandler : IRequestHandler<GetCashBoxDataQuery, PagedResult<CashBoxDto>>
{
    private readonly ICashBoxRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetCashBoxDataHandler(ICashBoxRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<CashBoxDto>> Handle(GetCashBoxDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(cb => cb.Branch)
            .Include(cb => cb.ChildAccount)
            .Where(cb => !cb.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        return new PagedResult<CashBoxDto>
        {
            Data = _mapper.Map<IEnumerable<CashBoxDto>>(paged.Data),
            TotalRecords = paged.TotalRecords,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalPages = paged.TotalPages,
            HasNextPage = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
