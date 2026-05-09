using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetPaymentVoucherByIdHandler : IRequestHandler<GetPaymentVoucherByIdQuery, PaymentVoucherDto?>
{
    private readonly IPaymentVoucherRepository _repository;
    private readonly IMapper _mapper;

    public GetPaymentVoucherByIdHandler(IPaymentVoucherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaymentVoucherDto?> Handle(GetPaymentVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithDetailsAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<PaymentVoucherDto>(entity);
    }
}
