using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Application.Queries.Accounting;

namespace Pharmacy.Api.Controllers.Accounting;

[Route("api/accounting/[controller]")]
[Authorize]
public class AccountingReportController(IMediator mediator, IAccountingReportService reportService) : BaseApiController
{
    /// <summary>
    /// قائمة الدخل / الأرباح والخسائر. Returns account lines, section totals,
    /// gross profit, and net profit for the selected inclusive date range.
    /// </summary>
    [HttpPost("ProfitAndLoss")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<IncomeStatementRowDto>>>> ProfitAndLoss(
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

    /// <summary>Income statement with gross debit/credit columns and section totals.</summary>
    [HttpPost("ProfitAndLossDebitCredit")]
    [HttpPost("ProfitAndLossStatemenDebitCreditReport")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<IncomeStatementRowDto>>>> ProfitAndLossDebitCredit(
        [FromBody] IncomeStatementRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await reportService.GetIncomeStatementAsync(request, cancellationToken);
            return SuccessResponse(result, "Income statement with debit and credit retrieved successfully");
        }
        catch (ArgumentException ex) { return ErrorResponse<IReadOnlyList<IncomeStatementRowDto>>(ex.Message, 400); }
        catch (Exception ex) { return ErrorResponse<IReadOnlyList<IncomeStatementRowDto>>($"Error retrieving income statement: {ex.Message}", 500); }
    }

    /// <summary>Balance sheet as of a date, without gross debit/credit columns.</summary>
    [HttpPost("balance-sheet")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BalanceSheetRowDto>>>> BalanceSheet(
        [FromBody] BalanceSheetRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await reportService.GetBalanceSheetAsync(request, cancellationToken);
            return SuccessResponse(result, "Balance sheet retrieved successfully");
        }
        catch (Exception ex) { return ErrorResponse<IReadOnlyList<BalanceSheetRowDto>>($"Error retrieving balance sheet: {ex.Message}", 500); }
    }

    /// <summary>Balance sheet as of a date, including gross debit/credit columns.</summary>
    [HttpPost("balance-sheet-debit-credit")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BalanceSheetDebitCreditRowDto>>>> BalanceSheetDebitCredit(
        [FromBody] BalanceSheetRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await reportService.GetBalanceSheetDebitCreditAsync(request, cancellationToken);
            return SuccessResponse(result, "Balance sheet with debit and credit retrieved successfully");
        }
        catch (Exception ex) { return ErrorResponse<IReadOnlyList<BalanceSheetDebitCreditRowDto>>($"Error retrieving balance sheet: {ex.Message}", 500); }
    }
}
