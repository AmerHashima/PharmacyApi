using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

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
        var entity = await _repository.GetByIdAsync(request.PaymentVoucher.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"PaymentVoucher '{request.PaymentVoucher.Oid}' not found");

        _mapper.Map(request.PaymentVoucher, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<PaymentVoucherDto>(entity);
    }
}
