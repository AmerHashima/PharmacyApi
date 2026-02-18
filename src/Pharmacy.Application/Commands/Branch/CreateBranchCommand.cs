using Pharmacy.Application.DTOs.Branch;
using MediatR;

namespace Pharmacy.Application.Commands.Branch;

/// <summary>
/// Command to create a new Branch
/// </summary>
public record CreateBranchCommand(CreateBranchDto Branch) : IRequest<BranchDto>;
