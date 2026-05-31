using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.CashierShift;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.CashierShift;

public class AddCashierShiftDetailHandler : IRequestHandler<AddCashierShiftDetailCommand, CashierShiftDetailDto>
{
    private readonly ICashierShiftRepository _shiftRepository;
    private readonly ICashierShiftDetailRepository _detailRepository;
    private readonly IMapper _mapper;

    public AddCashierShiftDetailHandler(
        ICashierShiftRepository shiftRepository,
        ICashierShiftDetailRepository detailRepository,
        IMapper mapper)
    {
        _shiftRepository = shiftRepository;
        _detailRepository = detailRepository;
        _mapper = mapper;
    }

    public async Task<CashierShiftDetailDto> Handle(AddCashierShiftDetailCommand request, CancellationToken cancellationToken)
    {
        var shift = await _shiftRepository.GetByIdAsync(request.Detail.ShiftId, cancellationToken)
            ?? throw new KeyNotFoundException($"Shift '{request.Detail.ShiftId}' not found.");

        if (shift.Status == 2)
            throw new InvalidOperationException($"Cannot add detail to a closed shift.");

        var detail = _mapper.Map<CashierShiftDetail>(request.Detail);
        detail.CreatedAt = DateTime.UtcNow;

        await _detailRepository.AddAsync(detail, cancellationToken);
        var created = await _detailRepository.GetByIdAsync(detail.Oid, cancellationToken);
        return _mapper.Map<CashierShiftDetailDto>(created!);
    }
}

public class DeleteCashierShiftDetailHandler : IRequestHandler<DeleteCashierShiftDetailCommand, bool>
{
    private readonly ICashierShiftDetailRepository _detailRepository;

    public DeleteCashierShiftDetailHandler(ICashierShiftDetailRepository detailRepository)
    {
        _detailRepository = detailRepository;
    }

    public async Task<bool> Handle(DeleteCashierShiftDetailCommand request, CancellationToken cancellationToken)
    {
        var detail = await _detailRepository.GetByIdAsync(request.DetailId, cancellationToken)
            ?? throw new KeyNotFoundException($"Shift detail '{request.DetailId}' not found.");

        await _detailRepository.DeleteAsync(detail.Oid, cancellationToken);
        return true;
    }
}
