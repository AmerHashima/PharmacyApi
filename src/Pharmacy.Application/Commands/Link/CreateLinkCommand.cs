using MediatR;
using Pharmacy.Application.DTOs.Link;

namespace Pharmacy.Application.Commands.Link;

public record CreateLinkCommand(CreateLinkDto Link) : IRequest<LinkDto>;
