using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreateJournalEntryHandler : IRequestHandler<CreateJournalEntryCommand, JournalEntryDto>
{
    private readonly IJournalEntryRepository _repository;
    private readonly IMapper _mapper;

    public CreateJournalEntryHandler(IJournalEntryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<JournalEntryDto> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        var master = _mapper.Map<JournalEntry>(request.JournalEntry);
        master.CreatedAt = DateTime.UtcNow;
        master.TotalDebit  = request.JournalEntry.Details.Sum(d => d.Debit);
        master.TotalCredit = request.JournalEntry.Details.Sum(d => d.Credit);

        var details = _mapper.Map<List<JournalEntryDetail>>(request.JournalEntry.Details);
        foreach (var detail in details)
        {
            detail.JournalEntryId = master.Oid;
            detail.CreatedAt = DateTime.UtcNow;
        }

        await _repository.InsertMasterDetailAsync(master, details, cancellationToken);

        var created = await _repository.GetWithDetailsAsync(master.Oid, cancellationToken);
        return _mapper.Map<JournalEntryDto>(created);
    }
}
