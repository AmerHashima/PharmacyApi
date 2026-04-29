using MediatR;
using Pharmacy.Application.DTOs.Link;

namespace Pharmacy.Application.Commands.Link;

public record UpdateLinkCommand(UpdateLinkDto Link) : IRequest<LinkDto>;
