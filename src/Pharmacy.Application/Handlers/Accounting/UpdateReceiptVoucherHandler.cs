using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using System.Linq.Expressions;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateReceiptVoucherHandler : IRequestHandler<UpdateReceiptVoucherCommand, ReceiptVoucherDto>
{
    private readonly IReceiptVoucherRepository _repository;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IMapper _mapper;

    public UpdateReceiptVoucherHandler(
        IReceiptVoucherRepository repository,
        IJournalEntryRepository journalEntryRepository,
        IMapper mapper)
    {
        _repository = repository;
        _journalEntryRepository = journalEntryRepository;
        _mapper = mapper;
    }

    public async Task<ReceiptVoucherDto> Handle(UpdateReceiptVoucherCommand request, CancellationToken cancellationToken)
    {
        var dto = request.ReceiptVoucher;

        var entity = await _repository.GetWithDetailsAsync(dto.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"ReceiptVoucher '{dto.Oid}' not found");

        _mapper.Map(dto, entity);
        entity.UpdatedAt   = DateTime.UtcNow;
        entity.TotalAmount = dto.Details.Sum(d => d.Amount);

        var details = _mapper.Map<List<ReceiptVoucherDetail>>(dto.Details);
        foreach (var detail in details)
        {
            detail.ReceiptVoucherId = entity.Oid;
            if (detail.CreatedAt == default) detail.CreatedAt = DateTime.UtcNow;
            detail.UpdatedAt = DateTime.UtcNow;
        }

        await _repository.UpdateMasterDetailAsync(
            entity,
            details,
            (Expression<Func<ReceiptVoucherDetail, object>>)(d => d.ReceiptVoucherId),
            cancellationToken);

        // ── Sync journal entry ───────────────────────────────────────────────
        if (entity.JournalEntryId.HasValue)
        {
            var journal = await _journalEntryRepository.GetWithDetailsAsync(entity.JournalEntryId.Value, cancellationToken);
            if (journal != null)
            {
                journal.EntryDate    = dto.VoucherDate;
                journal.FiscalYearId = dto.FiscalYearId;
                journal.BranchId     = dto.BranchId;
                journal.Description  = dto.Notes;
                journal.UpdatedAt    = DateTime.UtcNow;

                var journalDetails = dto.Details.Select(d => new JournalEntryDetail
                {
                    JournalEntryId = journal.Oid,
                    AccountId      = d.AccountId,
                    CostCenterId   = d.CostCenterId,
                    Description    = d.Description,
                    Debit          = d.Amount,
                    Credit         = 0,
                    CreatedAt      = DateTime.UtcNow,
                    UpdatedAt      = DateTime.UtcNow,
                }).ToList();

                journal.TotalDebit  = journalDetails.Sum(j => j.Debit);
                journal.TotalCredit = journalDetails.Sum(j => j.Credit);

                await _journalEntryRepository.UpdateMasterDetailAsync(
                    journal,
                    journalDetails,
                    (Expression<Func<JournalEntryDetail, object>>)(d => d.JournalEntryId),
                    cancellationToken);
            }
        }

        var updated = await _repository.GetWithDetailsAsync(entity.Oid, cancellationToken);
        return _mapper.Map<ReceiptVoucherDto>(updated);
    }
}
