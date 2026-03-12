using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.ProductUnit;
using Pharmacy.Application.Queries.ProductUnit;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.ProductUnit;

public class GetProductUnitDataHandler : IRequestHandler<GetProductUnitDataQuery, PagedResult<ProductUnitDto>>
{
    private readonly IProductUnitRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetProductUnitDataHandler(
        IProductUnitRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductUnitDto>> Handle(GetProductUnitDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(x => x.Product)
            .Include(x => x.PackageType)
            .Where(x => !x.IsDeleted);

        query = _queryBuilder.ApplyFilters(query, request.QueryRequest.Request.Filters);
        query = _queryBuilder.ApplySorting(query, request.QueryRequest.Request.Sort);

        var pagedResult = await _queryBuilder.ApplyPaginationAsync(query, request.QueryRequest.Request.Pagination);
        var dtos = _mapper.Map<List<ProductUnitDto>>(pagedResult.Data);

        return PagedResult<ProductUnitDto>.Create(
            dtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
