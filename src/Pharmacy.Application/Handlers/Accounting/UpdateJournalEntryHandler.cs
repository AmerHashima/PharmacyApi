using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateJournalEntryHandler : IRequestHandler<UpdateJournalEntryCommand, JournalEntryDto>
{
    private readonly IJournalEntryRepository _repository;
    private readonly IJournalEntryDetailRepository _detailRepository;
    private readonly IMapper _mapper;

    public UpdateJournalEntryHandler(
        IJournalEntryRepository repository,
        IJournalEntryDetailRepository detailRepository,
        IMapper mapper)
    {
        _repository = repository;
        _detailRepository = detailRepository;
        _mapper = mapper;
    }

    public async Task<JournalEntryDto> Handle(UpdateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithDetailsAsync(request.JournalEntry.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"JournalEntry '{request.JournalEntry.Oid}' not found");

        _mapper.Map(request.JournalEntry, entity);
        entity.UpdatedAt   = DateTime.UtcNow;
        entity.TotalDebit  = request.JournalEntry.Details.Sum(d => d.Debit);
        entity.TotalCredit = request.JournalEntry.Details.Sum(d => d.Credit);

        var newDetails = _mapper.Map<List<JournalEntryDetail>>(request.JournalEntry.Details);
        foreach (var detail in newDetails)
        {
            detail.JournalEntryId = entity.Oid;
            detail.CreatedAt = DateTime.UtcNow;
        }

        await _repository.UpdateMasterDetailAsync(entity, newDetails,
            d => d.JournalEntryId, cancellationToken);

        var updated = await _repository.GetWithDetailsAsync(entity.Oid, cancellationToken);
        return _mapper.Map<JournalEntryDto>(updated);
    }
}
