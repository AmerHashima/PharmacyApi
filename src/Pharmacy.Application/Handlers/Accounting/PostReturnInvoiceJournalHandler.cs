using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Manually posts one or more return invoices to the journal.
/// Each item is processed independently; failures are collected rather than aborting the batch.
/// </summary>
public class PostReturnInvoiceJournalHandler : IRequestHandler<PostReturnInvoiceJournalCommand, PostJournalBatchResultDto>
{
    private readonly IReturnInvoiceRepository _returnInvoiceRepository;
    private readonly ISalesInvoiceRepository _salesInvoiceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IAppLookupDetailRepository _lookupRepository;
    private readonly IFiscalYearRepository _fiscalYearRepository;
    private readonly IJournalPostingService _journalPostingService;
    private readonly IMapper _mapper;

    public PostReturnInvoiceJournalHandler(
        IReturnInvoiceRepository returnInvoiceRepository,
        ISalesInvoiceRepository salesInvoiceRepository,
        IProductRepository productRepository,
        IAppLookupDetailRepository lookupRepository,
        IFiscalYearRepository fiscalYearRepository,
        IJournalPostingService journalPostingService,
        IMapper mapper)
    {
        _returnInvoiceRepository = returnInvoiceRepository;
        _salesInvoiceRepository  = salesInvoiceRepository;
        _productRepository       = productRepository;
        _lookupRepository        = lookupRepository;
        _fiscalYearRepository    = fiscalYearRepository;
        _journalPostingService   = journalPostingService;
        _mapper                  = mapper;
    }

    public async Task<PostJournalBatchResultDto> Handle(PostReturnInvoiceJournalCommand request, CancellationToken cancellationToken)
    {
        var batch = new PostJournalBatchResultDto { TotalRequested = request.ReturnInvoiceIds.Count };

        foreach (var returnInvoiceId in request.ReturnInvoiceIds)
        {
            try
            {
                var entry = await PostSingleAsync(returnInvoiceId, cancellationToken);
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id           = returnInvoiceId,
                    Success      = true,
                    JournalEntry = entry
                });
                batch.TotalSucceeded++;
            }
            catch (Exception ex)
            {
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id      = returnInvoiceId,
                    Success = false,
                    Error   = ex.Message
                });
                batch.TotalFailed++;
            }
        }

        return batch;
    }

    private async Task<JournalEntryDto> PostSingleAsync(Guid returnInvoiceId, CancellationToken cancellationToken)
    {
        var returnInvoice = await _returnInvoiceRepository.GetWithItemsAsync(returnInvoiceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Return invoice '{returnInvoiceId}' not found");

        if (returnInvoice.JournalEntryId.HasValue)
            throw new InvalidOperationException($"Return invoice '{returnInvoice.ReturnNumber}' is already posted to journal entry '{returnInvoice.JournalEntryId}'");

        var originalInvoice = await _salesInvoiceRepository.GetWithItemsAsync(returnInvoice.OriginalInvoiceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Original invoice '{returnInvoice.OriginalInvoiceId}' not found");

        var paymentMethodCode = returnInvoice.PaymentMethodId.HasValue
            ? (await _lookupRepository.GetByIdAsync(returnInvoice.PaymentMethodId.Value, cancellationToken))?.ValueCode
            : null;

        Guid? fiscalYearId = returnInvoice.FiscalYearId;
        if (fiscalYearId == null)
        {
            var fy = await _fiscalYearRepository.GetCurrentAsync(cancellationToken);
            fiscalYearId = fy?.Oid;
        }

        var enhancedItems = new List<SalesInvoiceLineItem>();
        decimal totalTaxAmount = 0m;

        foreach (var item in returnInvoice.Items.Where(i => !i.IsDeleted))
        {
            var originalItem = item.OriginalInvoiceItemId.HasValue
                ? originalInvoice.Items.FirstOrDefault(i => i.Oid == item.OriginalInvoiceItemId.Value)
                : null;

            var taxPercent  = originalItem?.TaxPercent ?? 0m;
            var taxAmount   = originalItem?.TaxAmount ?? 0m;
            totalTaxAmount += taxAmount;

            var vatCategory = taxPercent switch
            {
                > 0 => VatCategory.Taxable,
                0 when originalItem?.TaxPercent.HasValue == true => VatCategory.ZeroRated,
                _ => VatCategory.Exempt
            };

            var product = item.Product ?? await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);

            enhancedItems.Add(new SalesInvoiceLineItem(
                ProductId:          item.ProductId,
                ProductName:        product?.DrugName ?? "Unknown",
                VatCategory:        vatCategory,
                Quantity:           item.Quantity,
                UnitPrice:          item.UnitPrice ?? 0m,
                LineDiscountAmount: item.DiscountAmount ?? 0m,
                NetPrice:           item.TotalPrice ?? 0m,
                TaxPercent:         taxPercent,
                TaxAmount:          taxAmount,
                TotalPrice:         (item.TotalPrice ?? 0m) + taxAmount,
                CostPrice:          originalItem?.CostPrice ?? 0m,
                LineNumber:         item.LineNumber,
                IsFreeItem:         false));
        }

        var refundTotal = (returnInvoice.TotalAmount ?? 0m) + totalTaxAmount;

        var refundMethods = new List<PaymentMethodDetail>();
        if (!string.IsNullOrEmpty(paymentMethodCode))
        {
            refundMethods.Add(new PaymentMethodDetail(
                MethodCode:    paymentMethodCode,
                Amount:        refundTotal,
                BankAccountId: null));
        }

        var postingRequest = new ReturnInvoicePostingRequest(
            ReturnInvoiceOid: returnInvoice.Oid,
            BranchId:         returnInvoice.BranchId,
            FiscalYearId:     fiscalYearId,
            ReturnNumber:     returnInvoice.ReturnNumber,
            ReturnDate:       returnInvoice.ReturnDate ?? DateTime.UtcNow,
            Items:            enhancedItems.AsReadOnly(),
            TotalAmount:      refundTotal,
            RefundMethods:    refundMethods.AsReadOnly(),
            CustomerId:       originalInvoice.CustomerId);

        var entry = await _journalPostingService.PostReturnInvoiceAsync(postingRequest, cancellationToken);
        return _mapper.Map<JournalEntryDto>(entry);
    }
}
