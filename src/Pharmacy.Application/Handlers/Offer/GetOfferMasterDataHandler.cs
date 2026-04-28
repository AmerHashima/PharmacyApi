using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Offer;
using Pharmacy.Application.Queries.Offer;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Offer;

public class GetOfferMasterDataHandler : IRequestHandler<GetOfferMasterDataQuery, PagedResult<OfferMasterDto>>
{
    private readonly IOfferMasterRepository _masterRepository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetOfferMasterDataHandler(IOfferMasterRepository masterRepository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _masterRepository = masterRepository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<OfferMasterDto>> Handle(GetOfferMasterDataQuery request, CancellationToken cancellationToken)
    {
        var query = _masterRepository.GetQueryable()
            .Include(o => o.OfferType)
            .Include(o => o.Branch)
            .Include(o => o.OfferDetails)
                .ThenInclude(d => d.Product)
            .Include(o => o.OfferDetails)
                .ThenInclude(d => d.FreeProduct)
            .Where(o => !o.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        var mapped = _mapper.Map<IEnumerable<OfferMasterDto>>(paged.Data);

        return new PagedResult<OfferMasterDto>
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
                { "availableFilters",    new List<string> { "OfferNameAr", "OfferNameEn", "StartDate", "EndDate" } },
                { "availableSortFields", new List<string> { "OfferNameAr", "OfferNameEn", "StartDate", "CreatedAt" } }
            }
        };
    }
}
