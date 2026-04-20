using MediatR;
using Pharmacy.Application.DTOs.GenericName;

namespace Pharmacy.Application.Commands.GenericName;

public record CreateGenericNameCommand(CreateGenericNameDto GenericName) : IRequest<GenericNameDto>;
