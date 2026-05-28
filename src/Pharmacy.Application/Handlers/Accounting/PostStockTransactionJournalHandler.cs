using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Manually posts one or more stock transactions to the journal.
/// Each item is processed independently; failures are collected rather than aborting the batch.
/// </summary>
public class PostStockTransactionJournalHandler : IRequestHandler<PostStockTransactionJournalCommand, PostJournalBatchResultDto>
{
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IFiscalYearRepository _fiscalYearRepository;
    private readonly IJournalPostingService _journalPostingService;
    private readonly IMapper _mapper;

    public PostStockTransactionJournalHandler(
        IStockTransactionRepository transactionRepository,
        IFiscalYearRepository fiscalYearRepository,
        IJournalPostingService journalPostingService,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _fiscalYearRepository  = fiscalYearRepository;
        _journalPostingService = journalPostingService;
        _mapper                = mapper;
    }

    public async Task<PostJournalBatchResultDto> Handle(PostStockTransactionJournalCommand request, CancellationToken cancellationToken)
    {
        var batch = new PostJournalBatchResultDto { TotalRequested = request.TransactionIds.Count };

        foreach (var transactionId in request.TransactionIds)
        {
            try
            {
                var entry = await PostSingleAsync(transactionId, cancellationToken);
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id           = transactionId,
                    Success      = true,
                    JournalEntry = entry
                });
                batch.TotalSucceeded++;
            }
            catch (Exception ex)
            {
                batch.Results.Add(new PostJournalItemResultDto
                {
                    Id      = transactionId,
                    Success = false,
                    Error   = ex.Message
                });
                batch.TotalFailed++;
            }
        }

        return batch;
    }

    private async Task<JournalEntryDto> PostSingleAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetWithDetailsAsync(transactionId, cancellationToken)
            ?? throw new KeyNotFoundException($"Stock transaction '{transactionId}' not found");

        if (transaction.JournalEntryId.HasValue)
            throw new InvalidOperationException($"Stock transaction '{transaction.ReferenceNumber}' is already posted to journal entry '{transaction.JournalEntryId}'");

        var typeCode = transaction.TransactionType?.ValueCode
            ?? throw new InvalidOperationException($"Stock transaction '{transactionId}' has no transaction type");

        var fy = await _fiscalYearRepository.GetCurrentAsync(cancellationToken);
        var fiscalYearId = fy?.Oid;

        decimal taxableNetCost   = 0;
        decimal zeroVatNetCost   = 0;
        decimal exemptNetCost    = 0;
        decimal taxableVatAmount = 0;
        var postingItems = new List<StockTransactionLineItem>();
        int lineIdx = 0;

        foreach (var d in transaction.Details.Where(d => !d.IsDeleted))
        {
            var grossCost = d.TotalCost ?? (d.Quantity * (d.UnitCost ?? 0));
            var taxAmount = d.TaxAmount ?? (grossCost * (d.TaxPercent ?? 0) / 100);
            var netCost   = d.NetCost ?? (grossCost - taxAmount);

            if (d.TaxPercent.HasValue && d.TaxPercent.Value > 0)
            {
                taxableNetCost   += netCost;
                taxableVatAmount += taxAmount;
            }
            else if (d.TaxPercent.HasValue)
            {
                zeroVatNetCost += netCost;
            }
            else
            {
                exemptNetCost += netCost;
            }

            postingItems.Add(new StockTransactionLineItem(
                ProductId:   d.ProductId,
                ProductName: d.Product?.DrugName ?? d.ProductId.ToString(),
                Quantity:    d.Quantity,
                UnitCost:    d.UnitCost ?? 0m,
                TotalCost:   grossCost,
                LineNumber:  d.LineNumber > 0 ? d.LineNumber : ++lineIdx));
        }

        var branchId = transaction.FromBranchId ?? transaction.ToBranchId
            ?? throw new InvalidOperationException("Stock transaction has no branch");

        var postingRequest = new StockTransactionPostingRequest(
            TransactionOid:   transaction.Oid,
            BranchId:         branchId,
            FiscalYearId:     fiscalYearId,
            ReferenceNumber:  transaction.ReferenceNumber ?? transaction.Oid.ToString(),
            TransactionDate:  transaction.TransactionDate ?? DateTime.UtcNow,
            TypeCode:         typeCode,
            Items:            postingItems,
            SupplierId:       transaction.SupplierId,
            TaxableNetCost:   taxableNetCost,
            ZeroVatNetCost:   zeroVatNetCost,
            ExemptNetCost:    exemptNetCost,
            TaxableVatAmount: taxableVatAmount,
            PayedAmount:      transaction.PayedAmount ?? 0m);

        var entry = await _journalPostingService.PostStockTransactionAsync(postingRequest, cancellationToken);
        return _mapper.Map<JournalEntryDto>(entry);
    }
}
