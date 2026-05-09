using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetReceiptVoucherByIdHandler : IRequestHandler<GetReceiptVoucherByIdQuery, ReceiptVoucherDto?>
{
    private readonly IReceiptVoucherRepository _repository;
    private readonly IMapper _mapper;

    public GetReceiptVoucherByIdHandler(IReceiptVoucherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ReceiptVoucherDto?> Handle(GetReceiptVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithDetailsAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<ReceiptVoucherDto>(entity);
    }
}
