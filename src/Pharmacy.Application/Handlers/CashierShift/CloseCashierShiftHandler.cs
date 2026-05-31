using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.CashierShift;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.CashierShift;

public class CloseCashierShiftHandler : IRequestHandler<CloseCashierShiftCommand, CashierShiftWithDetailsDto>
{
    private readonly ICashierShiftRepository _repository;
    private readonly ICashierShiftDetailRepository _detailRepository;
    private readonly IMapper _mapper;

    public CloseCashierShiftHandler(
        ICashierShiftRepository repository,
        ICashierShiftDetailRepository detailRepository,
        IMapper mapper)
    {
        _repository = repository;
        _detailRepository = detailRepository;
        _mapper = mapper;
    }

    public async Task<CashierShiftWithDetailsDto> Handle(CloseCashierShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await _repository.GetWithDetailsAsync(request.ShiftId, cancellationToken)
            ?? throw new KeyNotFoundException($"Shift '{request.ShiftId}' not found.");

        if (shift.Status == 2)
            throw new InvalidOperationException($"Shift '{shift.ShiftNumber}' is already closed.");

        var details = await _detailRepository.GetByShiftAsync(shift.Oid, cancellationToken);
        var totalIn  = details.Where(d => d.Amount > 0).Sum(d => d.Amount);
        var totalOut = details.Where(d => d.Amount < 0).Sum(d => d.Amount);

        shift.CloseDate       = request.CloseData.CloseDate;
        shift.ExpectedBalance = shift.OpeningBalance + totalIn + totalOut;
        shift.ActualBalance   = request.CloseData.ActualBalance ?? shift.ExpectedBalance;
        shift.DifferenceAmount = shift.ActualBalance - shift.ExpectedBalance;
        shift.Status          = 2; // Closed
        shift.Notes           = request.CloseData.Notes ?? shift.Notes;
        shift.UpdatedAt       = DateTime.UtcNow;

        await _repository.UpdateAsync(shift, cancellationToken);

        var updated = await _repository.GetWithDetailsAsync(shift.Oid, cancellationToken);
        return _mapper.Map<CashierShiftWithDetailsDto>(updated!);
    }
}
