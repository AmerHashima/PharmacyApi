using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using System.Linq.Expressions;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateReceiptVoucherHandler : IRequestHandler<UpdateReceiptVoucherCommand, ReceiptVoucherDto>
{
    private readonly IReceiptVoucherRepository _repository;
    private readonly IMapper _mapper;

    public UpdateReceiptVoucherHandler(IReceiptVoucherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ReceiptVoucherDto> Handle(UpdateReceiptVoucherCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithDetailsAsync(request.ReceiptVoucher.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"ReceiptVoucher '{request.ReceiptVoucher.Oid}' not found");

        _mapper.Map(request.ReceiptVoucher, entity);
        entity.UpdatedAt   = DateTime.UtcNow;
        entity.TotalAmount = request.ReceiptVoucher.Details.Sum(d => d.Amount);

        var details = _mapper.Map<List<ReceiptVoucherDetail>>(request.ReceiptVoucher.Details);
        foreach (var detail in details)
        {
            detail.ReceiptVoucherId = entity.Oid;
            if (detail.CreatedAt == default) detail.CreatedAt = DateTime.UtcNow;
            detail.UpdatedAt = DateTime.UtcNow;
        }

        await _repository.UpdateMasterDetailAsync(
            entity,
            details,
            (Expression<Func<ReceiptVoucherDetail, object>>)(d => d.ReceiptVoucherId),
            cancellationToken);

        var updated = await _repository.GetWithDetailsAsync(entity.Oid, cancellationToken);
        return _mapper.Map<ReceiptVoucherDto>(updated);
    }
}
