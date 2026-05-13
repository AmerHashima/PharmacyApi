using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

/// <summary>
/// Creates a child account under a given parent account and links it to a customer or stakeholder.
/// Exactly one of CustomerId / StakeholderId must be provided.
/// </summary>
public record CreateChildAccountCommand(CreateChildAccountDto Request) : IRequest<AccountDto>;
