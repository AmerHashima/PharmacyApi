using MediatR;

namespace Pharmacy.Application.Commands.Branch;

public record DeleteBranchLogoCommand(Guid BranchId) : IRequest<bool>;
