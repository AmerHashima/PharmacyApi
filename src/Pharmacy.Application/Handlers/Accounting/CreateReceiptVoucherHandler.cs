using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreateReceiptVoucherHandler : IRequestHandler<CreateReceiptVoucherCommand, ReceiptVoucherDto>
{
    private readonly IReceiptVoucherRepository _repository;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IVoucherNumberService _voucherNumberService;
    private readonly IMapper _mapper;

    public CreateReceiptVoucherHandler(
        IReceiptVoucherRepository repository,
        IJournalEntryRepository journalEntryRepository,
        IVoucherNumberService voucherNumberService,
        IMapper mapper)
    {
        _repository = repository;
        _journalEntryRepository = journalEntryRepository;
        _voucherNumberService = voucherNumberService;
        _mapper = mapper;
    }

    public async Task<ReceiptVoucherDto> Handle(CreateReceiptVoucherCommand request, CancellationToken cancellationToken)
    {
        var dto = request.ReceiptVoucher;

        var branchId = dto.BranchId
            ?? throw new InvalidOperationException("BranchId is required to generate a voucher number.");

        // ── 0. Generate voucher number ───────────────────────────────────────
        var voucherNumber = await _voucherNumberService.GenerateAsync(
            branchId, IVoucherNumberService.TypeReceipt, cancellationToken);

        // ── 1. Build and persist journal entry ──────────────────────────────
        var journalEntry = new JournalEntry
        {
            EntryNumber  = $"RV-{voucherNumber}",
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
            Debit          = d.Amount,
            Credit         = 0,
            CreatedAt      = DateTime.UtcNow,
        }).ToList();

        journalEntry.TotalDebit  = journalDetails.Sum(d => d.Debit);
        journalEntry.TotalCredit = journalDetails.Sum(d => d.Credit);

        await _journalEntryRepository.InsertMasterDetailAsync(journalEntry, journalDetails, cancellationToken);

        // ── 2. Build and persist receipt voucher ────────────────────────────
        var master = _mapper.Map<ReceiptVoucher>(dto);
        master.VoucherNumber  = voucherNumber;
        master.CreatedAt      = DateTime.UtcNow;
        master.TotalAmount    = dto.Details.Sum(d => d.Amount);
        master.JournalEntryId = journalEntry.Oid;

        var details = _mapper.Map<List<ReceiptVoucherDetail>>(dto.Details);
        foreach (var detail in details)
        {
            detail.ReceiptVoucherId = master.Oid;
            detail.CreatedAt        = DateTime.UtcNow;
        }

        await _repository.InsertMasterDetailAsync(master, details, cancellationToken);

        // ── 3. Patch journal ReferenceId → voucher Oid ──────────────────────
        journalEntry.ReferenceId = master.Oid;
        await _journalEntryRepository.UpdateAsync(journalEntry, cancellationToken);

        var created = await _repository.GetWithDetailsAsync(master.Oid, cancellationToken);
        return _mapper.Map<ReceiptVoucherDto>(created);
    }
}
