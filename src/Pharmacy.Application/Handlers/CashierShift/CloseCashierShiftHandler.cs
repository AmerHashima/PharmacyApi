using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.CashierShift;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Domain.Interfaces;
using System.Text.Json;

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

        if (request.CloseData.CloseDate < shift.OpenDate)
            throw new InvalidOperationException("Shift close date cannot be earlier than its open date.");

        if (request.CloseData.PaymentCounts.GroupBy(x => x.PaymentMethodId).Any(g => g.Count() > 1))
            throw new InvalidOperationException("Each payment method may be counted only once.");

        var details = await _detailRepository.GetByShiftAsync(shift.Oid, cancellationToken);
        var totalIn  = details.Where(d => d.Amount > 0).Sum(d => d.Amount);
        var totalOut = details.Where(d => d.Amount < 0).Sum(d => d.Amount);

        var expectedByMethod = details
            .GroupBy(d => new { d.PaymentMethodId, Name = d.PaymentMethod?.ValueNameEn ?? d.PaymentMethod?.ValueNameAr ?? "Unspecified" })
            .Select(g => new CashierShiftPaymentReconciliationDto
            {
                PaymentMethodId = g.Key.PaymentMethodId,
                PaymentMethodName = g.Key.Name,
                ExpectedAmount = g.Sum(x => x.Amount)
            })
            .ToDictionary(x => x.PaymentMethodId ?? Guid.Empty);

        var cashDetail = details.FirstOrDefault(d =>
            string.Equals(d.PaymentMethod?.ValueCode, "CASH", StringComparison.OrdinalIgnoreCase));
        var openingKey = cashDetail?.PaymentMethodId ?? Guid.Empty;
        if (!expectedByMethod.TryGetValue(openingKey, out var openingReconciliation))
        {
            openingReconciliation = new CashierShiftPaymentReconciliationDto
            {
                PaymentMethodId = cashDetail?.PaymentMethodId,
                PaymentMethodName = cashDetail?.PaymentMethod?.ValueNameEn
                    ?? cashDetail?.PaymentMethod?.ValueNameAr
                    ?? "Cash / Opening balance"
            };
            expectedByMethod[openingKey] = openingReconciliation;
        }
        openingReconciliation.ExpectedAmount += shift.OpeningBalance;

        foreach (var count in request.CloseData.PaymentCounts)
        {
            var paymentKey = count.PaymentMethodId ?? Guid.Empty;
            if (!expectedByMethod.TryGetValue(paymentKey, out var reconciliation))
            {
                reconciliation = new CashierShiftPaymentReconciliationDto
                {
                    PaymentMethodId = count.PaymentMethodId,
                    PaymentMethodName = count.PaymentMethodId?.ToString() ?? "Unspecified"
                };
                expectedByMethod[paymentKey] = reconciliation;
            }
            reconciliation.ActualAmount = count.ActualAmount;
        }

        // Backward compatibility for older clients that send one aggregate count.
        if (request.CloseData.PaymentCounts.Count == 0)
        {
            foreach (var reconciliation in expectedByMethod.Values)
                reconciliation.ActualAmount = reconciliation.ExpectedAmount;

            if (request.CloseData.ActualBalance.HasValue)
            {
                var aggregateExpected = expectedByMethod.Values.Sum(x => x.ExpectedAmount);
                openingReconciliation.ActualAmount += request.CloseData.ActualBalance.Value - aggregateExpected;
            }
        }

        foreach (var reconciliation in expectedByMethod.Values)
            reconciliation.DifferenceAmount = reconciliation.ActualAmount - reconciliation.ExpectedAmount;

        shift.CloseDate       = request.CloseData.CloseDate;
        shift.ExpectedBalance = shift.OpeningBalance + totalIn + totalOut;
        var countedTotal = request.CloseData.PaymentCounts.Count > 0
            ? request.CloseData.PaymentCounts.Sum(x => x.ActualAmount)
            : request.CloseData.ActualBalance;
        shift.ActualBalance   = countedTotal ?? shift.ExpectedBalance;
        shift.DifferenceAmount = shift.ActualBalance - shift.ExpectedBalance;
        shift.PaymentReconciliationJson = JsonSerializer.Serialize(expectedByMethod.Values.OrderBy(x => x.PaymentMethodName));
        shift.Status          = 2; // Closed
        shift.Notes           = request.CloseData.Notes ?? shift.Notes;
        shift.UpdatedAt       = DateTime.UtcNow;

        await _repository.UpdateAsync(shift, cancellationToken);

        var updated = await _repository.GetWithDetailsAsync(shift.Oid, cancellationToken);
        return _mapper.Map<CashierShiftWithDetailsDto>(updated!);
    }
}
