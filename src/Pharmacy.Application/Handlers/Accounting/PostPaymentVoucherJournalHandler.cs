using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Re-creates and links a journal entry for a payment voucher whose JournalEntryId is null.
/// Mirrors the same double-entry logic as <see cref="CreatePaymentVoucherHandler"/>:
///   CR each detail account  (the payable / expense accounts being paid)
///   DR the cash-box or bank account (the source of funds)
/// </summary>
public class PostPaymentVoucherJournalHandler : IRequestHandler<PostPaymentVoucherJournalCommand, JournalEntryDto>
{
    private readonly IPaymentVoucherRepository _voucherRepository;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IVoucherNumberService _voucherNumberService;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IAppLookupDetailRepository _lookupDetailRepository;
    private readonly IMapper _mapper;

    public PostPaymentVoucherJournalHandler(
        IPaymentVoucherRepository voucherRepository,
        IJournalEntryRepository journalEntryRepository,
        IVoucherNumberService voucherNumberService,
        ICashBoxRepository cashBoxRepository,
        IBankAccountRepository bankAccountRepository,
        IAppLookupDetailRepository lookupDetailRepository,
        IMapper mapper)
    {
        _voucherRepository    = voucherRepository;
        _journalEntryRepository = journalEntryRepository;
        _voucherNumberService = voucherNumberService;
        _cashBoxRepository    = cashBoxRepository;
        _bankAccountRepository = bankAccountRepository;
        _lookupDetailRepository = lookupDetailRepository;
        _mapper               = mapper;
    }

    public async Task<JournalEntryDto> Handle(PostPaymentVoucherJournalCommand request, CancellationToken cancellationToken)
    {
        var voucher = await _voucherRepository.GetWithDetailsAsync(request.VoucherId, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment voucher '{request.VoucherId}' not found");

        if (voucher.JournalEntryId.HasValue)
            throw new InvalidOperationException(
                $"Payment voucher '{voucher.VoucherNumber}' already has journal entry '{voucher.JournalEntryId}'");

        if (!voucher.Details.Any())
            throw new InvalidOperationException(
                $"Payment voucher '{voucher.VoucherNumber}' has no detail lines — cannot post");

        var branchId = voucher.BranchId
            ?? throw new InvalidOperationException("Payment voucher has no BranchId — cannot generate journal number");

        // Generate a new journal entry number
        var journalEntryNumber = await _voucherNumberService.GenerateJournalEntryNumberAsync(branchId, cancellationToken);

        // Resolve reference type
        var lookupDetails = await _lookupDetailRepository.GetByLookupCodeAsync("JOURNAL_REFERENCE_TYPE", cancellationToken);
        var referenceTypeId = lookupDetails.FirstOrDefault(d => d.ValueCode == IVoucherNumberService.PAYMENT_VOUCHER)?.Oid;

        // Build journal entry
        var journalEntry = new JournalEntry
        {
            EntryNumber     = journalEntryNumber,
            ReferenceTypeId = referenceTypeId,
            EntryDate       = voucher.VoucherDate,
            FiscalYearId    = voucher.FiscalYearId,
            BranchId        = voucher.BranchId,
            Description     = voucher.Notes,
            ReferenceId     = voucher.Oid,
            CreatedAt       = DateTime.UtcNow,
        };

        // CR each detail account (what is being paid out to)
        var journalDetails = voucher.Details.Select(d => new JournalEntryDetail
        {
            JournalEntryId = journalEntry.Oid,
            AccountId      = d.AccountId,
            CostCenterId   = d.CostCenterId,
            Description    = d.Description,
            Debit          = 0,
            Credit         = d.Amount,
            CreatedAt      = DateTime.UtcNow,
        }).ToList();

        // DR the bank / cashbox account (source of funds)
        var balancingAccountId = await ResolveBalancingAccountIdAsync(voucher.BankAccountId, voucher.CashBoxId, cancellationToken);
        if (balancingAccountId.HasValue)
        {
            var total = journalDetails.Sum(d => d.Credit);
            journalDetails.Add(new JournalEntryDetail
            {
                JournalEntryId = journalEntry.Oid,
                AccountId      = balancingAccountId.Value,
                Description    = voucher.Notes,
                Debit          = total,
                Credit         = 0,
                CreatedAt      = DateTime.UtcNow,
            });
        }

        journalEntry.TotalDebit  = journalDetails.Sum(d => d.Debit);
        journalEntry.TotalCredit = journalDetails.Sum(d => d.Credit);

        await _journalEntryRepository.InsertMasterDetailAsync(journalEntry, journalDetails, cancellationToken);

        // Link voucher → journal entry
        voucher.JournalEntryId = journalEntry.Oid;
        await _voucherRepository.UpdateAsync(voucher, cancellationToken);

        var created = await _journalEntryRepository.GetWithDetailsAsync(journalEntry.Oid, cancellationToken);
        return _mapper.Map<JournalEntryDto>(created);
    }

    private async Task<Guid?> ResolveBalancingAccountIdAsync(Guid? bankAccountId, Guid? cashBoxId, CancellationToken ct)
    {
        if (bankAccountId.HasValue)
        {
            var bank = await _bankAccountRepository.GetByIdAsync(bankAccountId.Value, ct);
            if (bank?.ChildAccountId != null) return bank.ChildAccountId;
        }
        if (cashBoxId.HasValue)
        {
            var cashBox = await _cashBoxRepository.GetByIdAsync(cashBoxId.Value, ct);
            if (cashBox?.ChildAccountId != null) return cashBox.ChildAccountId;
        }
        return null;
    }
}
