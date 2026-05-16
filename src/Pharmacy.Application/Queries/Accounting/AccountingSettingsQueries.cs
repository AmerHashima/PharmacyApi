using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.Accounting;

public record GetAccountingSettingsDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<AccountingSettingsDto>>;
public record GetAccountingSettingsByIdQuery(Guid Id) : IRequest<AccountingSettingsDto?>;
public record GetAccountingSettingsByBranchQuery(Guid BranchId) : IRequest<AccountingSettingsDto?>;
