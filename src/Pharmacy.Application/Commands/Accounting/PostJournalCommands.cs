using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

/// <summary>Manually post a sales invoice journal entry for branches where AutoPostJournal=false.</summary>
public record PostSalesInvoiceJournalCommand(Guid InvoiceId) : IRequest<JournalEntryDto>;

/// <summary>Manually post a return invoice journal entry for branches where AutoPostJournal=false.</summary>
public record PostReturnInvoiceJournalCommand(Guid ReturnInvoiceId) : IRequest<JournalEntryDto>;

/// <summary>Manually post a stock transaction journal entry for branches where AutoPostJournal=false.</summary>
public record PostStockTransactionJournalCommand(Guid TransactionId) : IRequest<JournalEntryDto>;

/// <summary>Validate that accounting accounts are configured for a branch and operation type.</summary>
public record ValidateBranchAccountingSetupQuery(Guid BranchId, string OperationType) : IRequest<AccountingValidationResultDto>;

/// <summary>
/// Re-creates and links a journal entry for a payment voucher that has no journal entry yet.
/// This handles cases where the voucher was saved but journal posting failed,
/// or the linked journal entry was manually deleted.
/// </summary>
public record PostPaymentVoucherJournalCommand(Guid VoucherId) : IRequest<JournalEntryDto>;

/// <summary>
/// Re-creates and links a journal entry for a receipt voucher that has no journal entry yet.
/// </summary>
public record PostReceiptVoucherJournalCommand(Guid VoucherId) : IRequest<JournalEntryDto>;
