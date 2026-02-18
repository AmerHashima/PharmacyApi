using Pharmacy.Application.DTOs.Branch;
using MediatR;

namespace Pharmacy.Application.Commands.Branch;

/// <summary>
/// Command to update an existing Branch
/// </summary>
public record UpdateBranchCommand(UpdateBranchDto Branch) : IRequest<BranchDto>;
