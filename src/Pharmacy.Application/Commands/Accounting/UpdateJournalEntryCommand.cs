using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record UpdateJournalEntryCommand(UpdateJournalEntryDto JournalEntry) : IRequest<JournalEntryDto>;
