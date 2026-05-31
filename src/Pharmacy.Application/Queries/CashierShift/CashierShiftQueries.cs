using MediatR;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.CashierShift;

public record GetCashierShiftByIdQuery(Guid Id) : IRequest<CashierShiftWithDetailsDto?>;

public record GetCashierShiftDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<CashierShiftDto>>;

public record GetCashierShiftsByBranchQuery(Guid BranchId) : IRequest<IEnumerable<CashierShiftDto>>;

public record GetCashierShiftsByUserQuery(Guid UserId) : IRequest<IEnumerable<CashierShiftDto>>;

public record GetOpenShiftQuery(Guid BranchId, Guid UserId) : IRequest<CashierShiftWithDetailsDto?>;
