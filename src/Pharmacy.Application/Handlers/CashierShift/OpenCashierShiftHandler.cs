using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.CashierShift;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.CashierShift;

public class OpenCashierShiftHandler : IRequestHandler<OpenCashierShiftCommand, CashierShiftWithDetailsDto>
{
    private readonly ICashierShiftRepository _repository;
    private readonly IMapper _mapper;

    public OpenCashierShiftHandler(ICashierShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CashierShiftWithDetailsDto> Handle(OpenCashierShiftCommand request, CancellationToken cancellationToken)
    {
        // Guard: no open shift for same user+branch
        var existing = await _repository.GetOpenShiftAsync(request.Shift.BranchId, request.Shift.UserId, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException($"User already has an open shift (ShiftNumber={existing.ShiftNumber}).");

        var shift = _mapper.Map<Domain.Entities.CashierShift>(request.Shift);
        shift.ShiftNumber = await GenerateShiftNumberAsync(cancellationToken);
        shift.Status = 1; // Open
        shift.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(shift, cancellationToken);

        var created = await _repository.GetWithDetailsAsync(shift.Oid, cancellationToken);
        return _mapper.Map<CashierShiftWithDetailsDto>(created!);
    }

    private async Task<string> GenerateShiftNumberAsync(CancellationToken ct)
    {
        var count = await _repository.CountAsync(ct);
        return $"SHIFT-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";
    }
}
