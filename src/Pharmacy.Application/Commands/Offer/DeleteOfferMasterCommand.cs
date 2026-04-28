using MediatR;

namespace Pharmacy.Application.Commands.Offer;

public record DeleteOfferMasterCommand(Guid Id) : IRequest<bool>;
