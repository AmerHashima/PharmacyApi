using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetJournalEntryByIdQuery(Guid Id) : IRequest<JournalEntryDto?>;
