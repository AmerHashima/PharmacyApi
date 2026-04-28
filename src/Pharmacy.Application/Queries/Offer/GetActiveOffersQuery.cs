using MediatR;
using Pharmacy.Application.DTOs.Offer;

namespace Pharmacy.Application.Queries.Offer;

public record GetActiveOffersQuery() : IRequest<IEnumerable<OfferMasterDto>>;
