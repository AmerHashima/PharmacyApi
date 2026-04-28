using MediatR;
using Pharmacy.Application.DTOs.Offer;

namespace Pharmacy.Application.Commands.Offer;

public record UpdateOfferMasterCommand(UpdateOfferMasterDto Offer) : IRequest<OfferMasterDto>;
