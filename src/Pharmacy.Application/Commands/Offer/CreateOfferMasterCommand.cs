using MediatR;
using Pharmacy.Application.DTOs.Offer;

namespace Pharmacy.Application.Commands.Offer;

public record CreateOfferMasterCommand(CreateOfferMasterDto Offer) : IRequest<OfferMasterDto>;
