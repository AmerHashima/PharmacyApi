using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.Accounting;

public record GetJournalEntryDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<JournalEntryDto>>;
