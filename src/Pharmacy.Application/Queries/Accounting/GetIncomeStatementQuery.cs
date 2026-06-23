using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

/// <summary>Returns the income statement / profit and loss report for a period.</summary>
public record GetIncomeStatementQuery(IncomeStatementRequest Request) : IRequest<IReadOnlyList<IncomeStatementRowDto>>;
