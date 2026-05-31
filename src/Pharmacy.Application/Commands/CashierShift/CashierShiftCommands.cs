using MediatR;
using Pharmacy.Application.DTOs.CashierShift;

namespace Pharmacy.Application.Commands.CashierShift;

public record OpenCashierShiftCommand(OpenCashierShiftDto Shift) : IRequest<CashierShiftWithDetailsDto>;

public record CloseCashierShiftCommand(Guid ShiftId, CloseCashierShiftDto CloseData) : IRequest<CashierShiftWithDetailsDto>;

public record AddCashierShiftDetailCommand(AddCashierShiftDetailDto Detail) : IRequest<CashierShiftDetailDto>;

public record DeleteCashierShiftDetailCommand(Guid DetailId) : IRequest<bool>;
