using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreatePaymentVoucherHandler : IRequestHandler<CreatePaymentVoucherCommand, PaymentVoucherDto>
{
    private readonly IPaymentVoucherRepository _repository;
    private readonly IMapper _mapper;

    public CreatePaymentVoucherHandler(IPaymentVoucherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaymentVoucherDto> Handle(CreatePaymentVoucherCommand request, CancellationToken cancellationToken)
    {
        var master = _mapper.Map<PaymentVoucher>(request.PaymentVoucher);
        master.CreatedAt   = DateTime.UtcNow;
        master.TotalAmount = request.PaymentVoucher.Details.Sum(d => d.Amount);

        var details = _mapper.Map<List<PaymentVoucherDetail>>(request.PaymentVoucher.Details);
        foreach (var detail in details)
        {
            detail.PaymentVoucherId = master.Oid;
            detail.CreatedAt = DateTime.UtcNow;
        }

        await _repository.InsertMasterDetailAsync(master, details, cancellationToken);

        var created = await _repository.GetWithDetailsAsync(master.Oid, cancellationToken);
        return _mapper.Map<PaymentVoucherDto>(created);
    }
}
