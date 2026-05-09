using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeleteJournalEntryCommand(Guid Id) : IRequest<bool>;
