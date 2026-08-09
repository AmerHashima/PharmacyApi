using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

/// <summary>Validate that accounting accounts are configured for a branch and operation type.</summary>
public record ValidateBranchAccountingSetupQuery(Guid BranchId, string OperationType) : IRequest<AccountingValidationResultDto>;
