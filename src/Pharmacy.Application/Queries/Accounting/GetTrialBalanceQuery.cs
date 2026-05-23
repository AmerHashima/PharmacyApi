using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

/// <summary>
/// Returns a full Trial Balance (ميزان المراجعة) for the given period and branch.
/// Supports filtering by parent account, leaf-only, and account level range.
/// </summary>
public record GetTrialBalanceQuery(TrialBalanceRequest Request) : IRequest<TrialBalanceReportDto>;
