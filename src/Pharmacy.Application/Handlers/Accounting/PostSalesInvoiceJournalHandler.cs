using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Manually posts one or more sales invoices to the journal.
/// Used for branches where AutoPostJournal=false — operator triggers posting explicitly.
/// Each item is processed independently; failures are collected rather than aborting the batch.
/// </summary>
public class PostSalesInvoiceJournalHandler : IRequestHandler<PostSalesInvoiceJournalCommand, PostJournalBatchResultDto>
{
    private readonly ISalesInvoiceRepository _invoiceRepository;
    private readonly IAppLookupDetailRepository _lookupRepository;
    private readonly IFiscalYearRepository _fiscalYearRepository;
    private readonly IJournalPostingService _journalPostingService;
    private readonly IMapper _mapper;

    public PostSalesInvoiceJournalHandler(
        ISalesInvoiceRepository invoiceRepository,
        IAppLookupDetailRepository lookupRepository,
        IFiscalYearRepository fiscalYearRepository,
        IJournalPostingService journalPostingService,
        IMapper mapper)
    {
        _invoiceRepository     = invoiceRepository;
        _lookupRepository      = lookupRepository;
        _fiscalYearRepository  = fiscalYearRepository;
        _journalPostingService = journalPostingService;
        _mapper                = mapper;
    }

    public async Task<PostJournalBatchResultDto> Handle(PostSalesInvoiceJournalCommand request, CancellationToken cancellationToken)
    {
        var batch = new PostJournalBatchResultDto { TotalRequested = request.InvoiceIds.Count };

        foreach (var invoiceId in request.InvoiceIds)
        {
            try
            {
                var entry = await PostSingleAsync(invoiceId, cancellationToken);
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id           = invoiceId,
                    Success      = true,
                    JournalEntry = entry
                });
                batch.TotalSucceeded++;
            }
            catch (Exception ex)
            {
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id      = invoiceId,
                    Success = false,
                    Error   = ex.Message
                });
                batch.TotalFailed++;
            }
        }

        return batch;
    }

    private async Task<JournalEntryDto> PostSingleAsync(Guid invoiceId, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetWithItemsAsync(invoiceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales invoice '{invoiceId}' not found");

        if (invoice.JournalEntryId.HasValue)
            throw new InvalidOperationException($"Invoice '{invoice.InvoiceNumber}' is already posted to journal entry '{invoice.JournalEntryId}'");

        var paymentMethodCode = invoice.PaymentMethodId.HasValue
            ? (await _lookupRepository.GetByIdAsync(invoice.PaymentMethodId.Value, cancellationToken))?.ValueCode
            : null;

        Guid? fiscalYearId = invoice.FiscalYearId;
        if (fiscalYearId == null)
        {
            var fy = await _fiscalYearRepository.GetCurrentAsync(cancellationToken);
            fiscalYearId = fy?.Oid;
        }

        var enhancedItems = invoice.Items
            .Where(i => !i.IsDeleted)
            .Select(i =>
            {
                var vatCategory = (i.TaxPercent ?? 0) switch
                {
                    > 0 => VatCategory.Taxable,
                    0 when i.TaxPercent.HasValue => VatCategory.ZeroRated,
                    _ => VatCategory.Exempt
                };

                return new SalesInvoiceLineItem(
                    ProductId:          i.ProductId,
                    ProductName:        i.Product?.DrugName ?? "Unknown",
                    VatCategory:        vatCategory,
                    Quantity:           i.Quantity,
                    UnitPrice:          i.UnitPrice ?? 0m,
                    LineDiscountAmount: i.DiscountAmount ?? 0m,
                    NetPrice:           i.NetPrice ?? 0m,
                    TaxPercent:         i.TaxPercent ?? 0m,
                    TaxAmount:          i.TaxAmount ?? 0m,
                    TotalPrice:         i.TotalPrice ?? 0m,
                    CostPrice:          i.CostPrice ?? 0m,
                    LineNumber:         i.LineNumber,
                    IsFreeItem:         i.IsFreeItem);
            })
            .ToList()
            .AsReadOnly();

        var payments = new List<PaymentMethodDetail>();
        if (!string.IsNullOrEmpty(paymentMethodCode))
        {
            payments.Add(new PaymentMethodDetail(
                MethodCode:    paymentMethodCode,
                Amount:        invoice.PaidAmount ?? invoice.TotalAmount ?? 0m,
                BankAccountId: null));
        }

        var postingRequest = new SalesInvoicePostingRequest(
            InvoiceOid:            invoice.Oid,
            BranchId:              invoice.BranchId,
            FiscalYearId:          fiscalYearId,
            InvoiceNumber:         invoice.InvoiceNumber,
            InvoiceDate:           invoice.InvoiceDate ?? DateTime.UtcNow,
            Items:                 enhancedItems,
            InvoiceDiscountAmount: invoice.DiscountAmount ?? 0m,
            TotalAmount:           invoice.TotalAmount ?? 0m,
            Payments:              payments.AsReadOnly(),
            CustomerId:            invoice.CustomerId);

        var result = await _journalPostingService.PostSalesInvoiceAsync(postingRequest, cancellationToken);
        return _mapper.Map<JournalEntryDto>(result.Entry);
    }
}
