using MediatR;
using Pharmacy.Application.DTOs.GenericName;

namespace Pharmacy.Application.Queries.GenericName;

public record SearchGenericNamesQuery(string Term) : IRequest<IEnumerable<GenericNameDto>>;
