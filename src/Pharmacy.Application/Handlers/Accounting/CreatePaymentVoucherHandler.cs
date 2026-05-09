using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
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
        var entity = _mapper.Map<Domain.Entities.Accounting.PaymentVoucher>(request.PaymentVoucher);
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<PaymentVoucherDto>(entity);
    }
}
