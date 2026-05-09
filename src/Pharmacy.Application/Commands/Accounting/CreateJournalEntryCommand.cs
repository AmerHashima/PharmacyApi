using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreateJournalEntryCommand(CreateJournalEntryDto JournalEntry) : IRequest<JournalEntryDto>;
