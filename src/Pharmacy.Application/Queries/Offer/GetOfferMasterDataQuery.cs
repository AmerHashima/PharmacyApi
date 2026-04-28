using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Offer;

namespace Pharmacy.Application.Queries.Offer;

public record GetOfferMasterDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<OfferMasterDto>>;
