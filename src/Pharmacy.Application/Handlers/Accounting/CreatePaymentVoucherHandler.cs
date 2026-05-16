using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreatePaymentVoucherHandler : IRequestHandler<CreatePaymentVoucherCommand, PaymentVoucherDto>
{
    private readonly IPaymentVoucherRepository _repository;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IVoucherNumberService _voucherNumberService;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IAppLookupDetailRepository _lookupDetailRepository;
    private readonly IMapper _mapper;

    public CreatePaymentVoucherHandler(
        IPaymentVoucherRepository repository,
        IJournalEntryRepository journalEntryRepository,
        IVoucherNumberService voucherNumberService,
        ICashBoxRepository cashBoxRepository,
        IBankAccountRepository bankAccountRepository,
        IAppLookupDetailRepository lookupDetailRepository,
        IMapper mapper)
    {
        _repository = repository;
        _journalEntryRepository = journalEntryRepository;
        _voucherNumberService = voucherNumberService;
        _cashBoxRepository = cashBoxRepository;
        _bankAccountRepository = bankAccountRepository;
        _lookupDetailRepository = lookupDetailRepository;
        _mapper = mapper;
    }

    public async Task<PaymentVoucherDto> Handle(CreatePaymentVoucherCommand request, CancellationToken cancellationToken)
    {
        var dto = request.PaymentVoucher;

        var branchId = dto.BranchId
            ?? throw new InvalidOperationException("BranchId is required to generate a voucher number.");

        // ── 0. Generate voucher number & journal entry number (independent sequences) ──
        var voucherNumber = await _voucherNumberService.GenerateAsync(
            branchId, IVoucherNumberService.TypePayment, cancellationToken);
        var journalEntryNumber = await _voucherNumberService.GenerateJournalEntryNumberAsync(
            branchId, cancellationToken);

        // Resolve ReferenceTypeId from AppLookup (LookupCode=JOURNAL_REFERENCE_TYPE, ValueCode=PAYMENT_VOUCHER)
        var lookupDetails = await _lookupDetailRepository
            .GetByLookupCodeAsync("JOURNAL_REFERENCE_TYPE", cancellationToken);
        var referenceTypeId = lookupDetails
            .FirstOrDefault(d => d.ValueCode == IVoucherNumberService.PAYMENT_VOUCHER)?.Oid;

        // ── 1. Build and persist journal entry ──────────────────────────────
        var journalEntry = new JournalEntry
        {
            EntryNumber     = journalEntryNumber,
            ReferenceTypeId = referenceTypeId,
            EntryDate    = dto.VoucherDate,
            FiscalYearId = dto.FiscalYearId,
            BranchId     = dto.BranchId,
            Description  = dto.Notes,
            ReferenceId  = null,
            CreatedAt    = DateTime.UtcNow,
        };

        var journalDetails = dto.Details.Select(d => new JournalEntryDetail
        {
            JournalEntryId = journalEntry.Oid,
            AccountId      = d.AccountId,
            CostCenterId   = d.CostCenterId,
            Description    = d.Description,
            Debit          = 0,
            Credit         = d.Amount,
            CreatedAt      = DateTime.UtcNow,
        }).ToList();

        // ── Balancing row: Debit the bank or cashbox account ────────────────
        var balancingAccountId = await ResolveBalancingAccountIdAsync(
            dto.BankAccountId, dto.CashBoxId, cancellationToken);

        if (balancingAccountId.HasValue)
        {
            var total = journalDetails.Sum(d => d.Credit);
            journalDetails.Add(new JournalEntryDetail
            {
                JournalEntryId = journalEntry.Oid,
                AccountId      = balancingAccountId.Value,
                Description    = dto.Notes,
                Debit          = total,
                Credit         = 0,
                CreatedAt      = DateTime.UtcNow,
            });
        }

        journalEntry.TotalDebit  = journalDetails.Sum(d => d.Debit);
        journalEntry.TotalCredit = journalDetails.Sum(d => d.Credit);

        await _journalEntryRepository.InsertMasterDetailAsync(journalEntry, journalDetails, cancellationToken);

        // ── 2. Build and persist payment voucher ────────────────────────────
        var master = _mapper.Map<PaymentVoucher>(dto);
        master.VoucherNumber  = voucherNumber;
        master.CreatedAt      = DateTime.UtcNow;
        master.TotalAmount    = dto.Details.Sum(d => d.Amount);
        master.JournalEntryId = journalEntry.Oid;

        var details = _mapper.Map<List<PaymentVoucherDetail>>(dto.Details);
        foreach (var detail in details)
        {
            detail.PaymentVoucherId = master.Oid;
            detail.CreatedAt        = DateTime.UtcNow;
        }

        await _repository.InsertMasterDetailAsync(master, details, cancellationToken);

        // ── 3. Patch journal ReferenceId → voucher Oid ──────────────────────
        journalEntry.ReferenceId = master.Oid;
        await _journalEntryRepository.UpdateAsync(journalEntry, cancellationToken);

        var created = await _repository.GetWithDetailsAsync(master.Oid, cancellationToken);
        return _mapper.Map<PaymentVoucherDto>(created);
    }

    private async Task<Guid?> ResolveBalancingAccountIdAsync(
        Guid? bankAccountId, Guid? cashBoxId, CancellationToken ct)
    {
        if (bankAccountId.HasValue)
        {
            var bank = await _bankAccountRepository.GetByIdAsync(bankAccountId.Value, ct);
            if (bank?.AccountId != null) return bank.AccountId;
        }
        if (cashBoxId.HasValue)
        {
            var cashBox = await _cashBoxRepository.GetByIdAsync(cashBoxId.Value, ct);
            if (cashBox?.AccountId != null) return cashBox.AccountId;
        }
        return null;
    }
}
