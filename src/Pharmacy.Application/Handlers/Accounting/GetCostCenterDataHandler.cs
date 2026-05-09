using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetCostCenterDataHandler : IRequestHandler<GetCostCenterDataQuery, PagedResult<CostCenterDto>>
{
    private readonly ICostCenterRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetCostCenterDataHandler(ICostCenterRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<CostCenterDto>> Handle(GetCostCenterDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(c => c.Parent)
            .Where(c => !c.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        return new PagedResult<CostCenterDto>
        {
            Data = _mapper.Map<IEnumerable<CostCenterDto>>(paged.Data),
            TotalRecords = paged.TotalRecords,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalPages = paged.TotalPages,
            HasNextPage = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
