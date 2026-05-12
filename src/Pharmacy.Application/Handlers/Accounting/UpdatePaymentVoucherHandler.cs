using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using System.Linq.Expressions;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdatePaymentVoucherHandler : IRequestHandler<UpdatePaymentVoucherCommand, PaymentVoucherDto>
{
    private readonly IPaymentVoucherRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePaymentVoucherHandler(IPaymentVoucherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaymentVoucherDto> Handle(UpdatePaymentVoucherCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithDetailsAsync(request.PaymentVoucher.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"PaymentVoucher '{request.PaymentVoucher.Oid}' not found");

        _mapper.Map(request.PaymentVoucher, entity);
        entity.UpdatedAt   = DateTime.UtcNow;
        entity.TotalAmount = request.PaymentVoucher.Details.Sum(d => d.Amount);

        var details = _mapper.Map<List<PaymentVoucherDetail>>(request.PaymentVoucher.Details);
        foreach (var detail in details)
        {
            detail.PaymentVoucherId = entity.Oid;
            if (detail.CreatedAt == default) detail.CreatedAt = DateTime.UtcNow;
            detail.UpdatedAt = DateTime.UtcNow;
        }

        await _repository.UpdateMasterDetailAsync(
            entity,
            details,
            (Expression<Func<PaymentVoucherDetail, object>>)(d => d.PaymentVoucherId),
            cancellationToken);

        var updated = await _repository.GetWithDetailsAsync(entity.Oid, cancellationToken);
        return _mapper.Map<PaymentVoucherDto>(updated);
    }
}
