using MediatR;
using Pharmacy.Application.DTOs.Link;

namespace Pharmacy.Application.Queries.Link;

public record GetLinkByIdQuery(Guid Id) : IRequest<LinkDto?>;
