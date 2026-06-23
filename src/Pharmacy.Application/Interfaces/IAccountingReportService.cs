using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Interfaces;

public interface IAccountingReportService
{
    Task<IReadOnlyList<IncomeStatementRowDto>> GetIncomeStatementAsync(IncomeStatementRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BalanceSheetRowDto>> GetBalanceSheetAsync(BalanceSheetRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BalanceSheetDebitCreditRowDto>> GetBalanceSheetDebitCreditAsync(BalanceSheetRequest request, CancellationToken cancellationToken = default);
}
