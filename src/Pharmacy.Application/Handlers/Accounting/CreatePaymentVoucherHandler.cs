using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreatePaymentVoucherHandler : IRequestHandler<CreatePaymentVoucherCommand, PaymentVoucherDto>
{
    private readonly IPaymentVoucherRepository _repository;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentVoucherHandler(
        IPaymentVoucherRepository repository,
        IJournalEntryRepository journalEntryRepository,
        IMapper mapper)
    {
        _repository = repository;
        _journalEntryRepository = journalEntryRepository;
        _mapper = mapper;
    }

    public async Task<PaymentVoucherDto> Handle(CreatePaymentVoucherCommand request, CancellationToken cancellationToken)
    {
        var dto = request.PaymentVoucher;

        // ── 1. Build and persist journal entry ──────────────────────────────
        var journalEntry = new JournalEntry
        {
            EntryNumber   = $"PV-{dto.VoucherNumber ?? DateTime.UtcNow.Ticks.ToString()}",
            EntryDate     = dto.VoucherDate,
            FiscalYearId  = dto.FiscalYearId,
            BranchId      = dto.BranchId,
            Description   = dto.Notes,
            ReferenceId   = null,          // patched after voucher save
            CreatedAt     = DateTime.UtcNow,
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

        journalEntry.TotalDebit  = journalDetails.Sum(d => d.Debit);
        journalEntry.TotalCredit = journalDetails.Sum(d => d.Credit);

        await _journalEntryRepository.InsertMasterDetailAsync(journalEntry, journalDetails, cancellationToken);

        // ── 2. Build and persist payment voucher ────────────────────────────
        var master = _mapper.Map<PaymentVoucher>(dto);
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
}
