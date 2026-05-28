using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Manually posts a stock transaction to the journal.
/// Used for branches where AutoPostJournal=false — operator triggers posting explicitly.
/// </summary>
public class PostStockTransactionJournalHandler : IRequestHandler<PostStockTransactionJournalCommand, JournalEntryDto>
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

    public async Task<JournalEntryDto> Handle(PostStockTransactionJournalCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetWithDetailsAsync(request.TransactionId, cancellationToken)
            ?? throw new KeyNotFoundException($"Stock transaction '{request.TransactionId}' not found");

        if (transaction.JournalEntryId.HasValue)
            throw new InvalidOperationException($"Stock transaction '{transaction.ReferenceNumber}' is already posted to journal entry '{transaction.JournalEntryId}'");

        var typeCode = transaction.TransactionType?.ValueCode
            ?? throw new InvalidOperationException($"Stock transaction '{request.TransactionId}' has no transaction type");

        // Fiscal year
        Guid? fiscalYearId = null;
        var fy = await _fiscalYearRepository.GetCurrentAsync(cancellationToken);
        fiscalYearId = fy?.Oid;

        // Recompute VAT breakdown from persisted detail lines
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
