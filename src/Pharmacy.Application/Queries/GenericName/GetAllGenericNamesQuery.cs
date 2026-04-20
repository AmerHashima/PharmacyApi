using MediatR;
using Pharmacy.Application.DTOs.GenericName;

namespace Pharmacy.Application.Queries.GenericName;

public record GetAllGenericNamesQuery() : IRequest<IEnumerable<GenericNameDto>>;
