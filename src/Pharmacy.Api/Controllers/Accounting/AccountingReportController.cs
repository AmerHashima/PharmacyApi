using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;

namespace Pharmacy.Api.Controllers.Accounting;

[Route("api/accounting/[controller]")]
[Authorize]
public class AccountingReportController(IMediator mediator) : BaseApiController
{
    /// <summary>
    /// قائمة الدخل / الأرباح والخسائر. Returns account lines, section totals,
    /// gross profit, and net profit for the selected inclusive date range.
    /// </summary>
    [HttpPost("income-statement")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<IncomeStatementRowDto>>>> IncomeStatement(
        [FromBody] IncomeStatementRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new GetIncomeStatementQuery(request), cancellationToken);
            return SuccessResponse(result, "Income statement retrieved successfully");
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse<IReadOnlyList<IncomeStatementRowDto>>(ex.Message, 400);
        }
        catch (Exception ex)
        {
            return ErrorResponse<IReadOnlyList<IncomeStatementRowDto>>($"Error retrieving income statement: {ex.Message}", 500);
        }
    }
}
