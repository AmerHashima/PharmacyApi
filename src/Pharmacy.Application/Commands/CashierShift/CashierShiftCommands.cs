using MediatR;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Application.Common.Interfaces;

namespace Pharmacy.Application.Commands.CashierShift;

public record OpenCashierShiftCommand(OpenCashierShiftDto Shift) : IRequest<CashierShiftWithDetailsDto>;

public record CloseCashierShiftCommand(Guid ShiftId, CloseCashierShiftDto CloseData) : IRequest<CashierShiftWithDetailsDto>, ITransactionalRequest;

public record AddCashierShiftDetailCommand(AddCashierShiftDetailDto Detail) : IRequest<CashierShiftDetailDto>;

public record DeleteCashierShiftDetailCommand(Guid DetailId) : IRequest<bool>;
