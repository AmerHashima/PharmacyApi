using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Manually posts one or more stock transaction returns to the journal.
/// Each item is processed independently; failures are collected rather than aborting the batch.
/// </summary>
public class PostStockTransactionReturnJournalHandler
    : IRequestHandler<PostStockTransactionReturnJournalCommand, PostJournalBatchResultDto>
{
    private readonly IStockTransactionReturnRepository _returnRepository;
    private readonly IFiscalYearRepository _fiscalYearRepository;
    private readonly IJournalPostingService _journalPostingService;
    private readonly IMapper _mapper;

    public PostStockTransactionReturnJournalHandler(
        IStockTransactionReturnRepository returnRepository,
        IFiscalYearRepository fiscalYearRepository,
        IJournalPostingService journalPostingService,
        IMapper mapper)
    {
        _returnRepository     = returnRepository;
        _fiscalYearRepository = fiscalYearRepository;
        _journalPostingService = journalPostingService;
        _mapper               = mapper;
    }

    public async Task<PostJournalBatchResultDto> Handle(
        PostStockTransactionReturnJournalCommand request,
        CancellationToken cancellationToken)
    {
        var batch = new PostJournalBatchResultDto { TotalRequested = request.ReturnIds.Count };

        foreach (var returnId in request.ReturnIds)
        {
            try
            {
                var entry = await PostSingleAsync(returnId, cancellationToken);
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id           = returnId,
                    Success      = true,
                    JournalEntry = entry
                });
                batch.TotalSucceeded++;
            }
            catch (Exception ex)
            {
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id      = returnId,
                    Success = false,
                    Error   = ex.Message
                });
                batch.TotalFailed++;
            }
        }

        return batch;
    }

    private async Task<JournalEntryDto> PostSingleAsync(Guid returnId, CancellationToken cancellationToken)
    {
        var returnTx = await _returnRepository.GetWithDetailsAsync(returnId, cancellationToken)
            ?? throw new KeyNotFoundException($"Stock transaction return '{returnId}' not found.");

        if (returnTx.JournalEntryId.HasValue)
            throw new InvalidOperationException(
                $"Stock transaction return '{returnTx.ReferenceNumber}' is already posted to journal entry '{returnTx.JournalEntryId}'.");

        var typeCode = returnTx.TransactionType?.ValueCode
            ?? throw new InvalidOperationException(
                $"Stock transaction return '{returnId}' has no transaction type.");

        var fy = await _fiscalYearRepository.GetCurrentAsync(cancellationToken);

        decimal taxableNetCost   = 0;
        decimal zeroVatNetCost   = 0;
        decimal exemptNetCost    = 0;
        decimal taxableVatAmount = 0;
        var postingItems = new List<StockTransactionLineItem>();
        int lineIdx = 0;

        foreach (var d in returnTx.Details.Where(d => !d.IsDeleted))
        {
            var grossCost = d.TotalCost ?? (d.Quantity * (d.UnitCost ?? 0));
            // Return details have no VAT breakdown — treat as exempt (no VAT line)
            exemptNetCost += grossCost;

            postingItems.Add(new StockTransactionLineItem(
                ProductId:   d.ProductId,
                ProductName: d.Product?.DrugName ?? d.ProductId.ToString(),
                Quantity:    d.Quantity,
                UnitCost:    d.UnitCost ?? 0m,
                TotalCost:   grossCost,
                LineNumber:  d.LineNumber > 0 ? d.LineNumber : ++lineIdx));
        }

        var branchId = returnTx.FromBranchId ?? returnTx.ToBranchId
            ?? throw new InvalidOperationException("Stock transaction return has no branch.");

        var postingRequest = new StockTransactionReturnPostingRequest(
            ReturnOid:        returnTx.Oid,
            BranchId:         branchId,
            FiscalYearId:     fy?.Oid,
            ReferenceNumber:  returnTx.ReferenceNumber ?? returnTx.Oid.ToString(),
            TransactionDate:  returnTx.TransactionDate ?? DateTime.UtcNow,
            TypeCode:         typeCode,
            Items:            postingItems,
            SupplierId:       returnTx.SupplierId,
            TaxableNetCost:   taxableNetCost,
            ZeroVatNetCost:   zeroVatNetCost,
            ExemptNetCost:    exemptNetCost,
            TaxableVatAmount: taxableVatAmount,
            PayedAmount:      0m);

        var entry = await _journalPostingService.PostStockTransactionReturnAsync(postingRequest, cancellationToken);
        return _mapper.Map<JournalEntryDto>(entry);
    }
}
