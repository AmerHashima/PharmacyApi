using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Handler delegates income statement generation to an infrastructure service
/// that executes a literal SQL query. Filters are applied in-database by the service.
/// </summary>
public class GetIncomeStatementHandler : IRequestHandler<GetIncomeStatementQuery, IReadOnlyList<IncomeStatementRowDto>>
{
    private readonly IAccountingReportService _reportService;

    public GetIncomeStatementHandler(IAccountingReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IReadOnlyList<IncomeStatementRowDto>> Handle(GetIncomeStatementQuery request, CancellationToken cancellationToken)
    {
        return await _reportService.GetIncomeStatementAsync(request.Request, cancellationToken);
    }
}
