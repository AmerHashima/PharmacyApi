using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Validates that all required accounting accounts are configured for a branch and operation type.
/// Never throws — returns a structured result with IsValid and a list of missing accounts.
/// </summary>
public class ValidateBranchAccountingSetupHandler : IRequestHandler<ValidateBranchAccountingSetupQuery, AccountingValidationResultDto>
{
    private readonly IJournalPostingService _journalPostingService;

    public ValidateBranchAccountingSetupHandler(IJournalPostingService journalPostingService)
    {
        _journalPostingService = journalPostingService;
    }

    public async Task<AccountingValidationResultDto> Handle(ValidateBranchAccountingSetupQuery request, CancellationToken cancellationToken)
    {
        var result = new AccountingValidationResultDto
        {
            BranchId      = request.BranchId,
            OperationType = request.OperationType.ToUpperInvariant()
        };

        try
        {
            switch (result.OperationType)
            {
                case "SALES":
                    await _journalPostingService.ValidateSalesAccountingSetupAsync(request.BranchId, cancellationToken);
                    break;

                case "RETURN":
                    await _journalPostingService.ValidateReturnAccountingSetupAsync(request.BranchId, cancellationToken);
                    break;

                case "IN":
                case "OUT":
                case "TRANSFER":
                case "RETURN_PURCHASE":
                case "ADJUSTMENT":
                case "EXPIRED":
                case "DAMAGED":
                    await _journalPostingService.ValidateStockTransactionAccountingSetupAsync(
                        request.BranchId, result.OperationType, cancellationToken);
                    break;

                default:
                    result.IsValid  = false;
                    result.Message  = $"Unknown operation type '{request.OperationType}'. Valid values: SALES, RETURN, IN, OUT, TRANSFER, ADJUSTMENT, EXPIRED, DAMAGED.";
                    return result;
            }

            result.IsValid = true;
            result.Message = $"Accounting setup for '{result.OperationType}' is complete.";
        }
        catch (InvalidOperationException ex)
        {
            result.IsValid = false;
            result.Message = ex.Message;

            // Parse missing account names from the exception message lines
            var lines = ex.Message
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Where(l => l.StartsWith('-') || l.StartsWith('•') || l.StartsWith('*'))
                .Select(l => l.TrimStart('-', '•', '*', ' '))
                .ToList();

            result.MissingAccounts = lines.Count > 0 ? lines : [ex.Message];
        }

        return result;
    }
}
