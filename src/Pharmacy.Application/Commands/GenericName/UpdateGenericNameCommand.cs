using MediatR;
using Pharmacy.Application.DTOs.GenericName;

namespace Pharmacy.Application.Commands.GenericName;

public record UpdateGenericNameCommand(UpdateGenericNameDto GenericName) : IRequest<GenericNameDto>;
