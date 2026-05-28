using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

/// <summary>Manually post journal entries for a list of sales invoices.</summary>
public record PostSalesInvoiceJournalCommand(List<Guid> InvoiceIds) : IRequest<PostJournalBatchResultDto>;

/// <summary>Manually post journal entries for a list of return invoices.</summary>
public record PostReturnInvoiceJournalCommand(List<Guid> ReturnInvoiceIds) : IRequest<PostJournalBatchResultDto>;

/// <summary>Manually post journal entries for a list of stock transactions.</summary>
public record PostStockTransactionJournalCommand(List<Guid> TransactionIds) : IRequest<PostJournalBatchResultDto>;

/// <summary>Validate that accounting accounts are configured for a branch and operation type.</summary>
public record ValidateBranchAccountingSetupQuery(Guid BranchId, string OperationType) : IRequest<AccountingValidationResultDto>;

/// <summary>Re-creates and links journal entries for a list of payment vouchers.</summary>
public record PostPaymentVoucherJournalCommand(List<Guid> VoucherIds) : IRequest<PostJournalBatchResultDto>;

/// <summary>Re-creates and links journal entries for a list of receipt vouchers.</summary>
public record PostReceiptVoucherJournalCommand(List<Guid> VoucherIds) : IRequest<PostJournalBatchResultDto>;

