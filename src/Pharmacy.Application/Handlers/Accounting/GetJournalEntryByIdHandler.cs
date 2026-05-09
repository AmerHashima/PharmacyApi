using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetJournalEntryByIdHandler : IRequestHandler<GetJournalEntryByIdQuery, JournalEntryDto?>
{
    private readonly IJournalEntryRepository _repository;
    private readonly IMapper _mapper;

    public GetJournalEntryByIdHandler(IJournalEntryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<JournalEntryDto?> Handle(GetJournalEntryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithDetailsAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<JournalEntryDto>(entity);
    }
}
