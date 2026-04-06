using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Store;
using Pharmacy.Application.Queries.Store;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Store;

public class GetStoreDataHandler : IRequestHandler<GetStoreDataQuery, PagedResult<StoreDto>>
{
    private readonly IStoreRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetStoreDataHandler(IStoreRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<StoreDto>> Handle(GetStoreDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(s => s.Branch)
            .Where(s => !s.IsDeleted);

        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        var pagedResult = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);

        var dtos = _mapper.Map<List<StoreDto>>(pagedResult.Data);

        return PagedResult<StoreDto>.Create(dtos, pagedResult.TotalRecords, pagedResult.PageNumber, pagedResult.PageSize);
    }
}
