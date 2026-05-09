using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreateReceiptVoucherHandler : IRequestHandler<CreateReceiptVoucherCommand, ReceiptVoucherDto>
{
    private readonly IReceiptVoucherRepository _repository;
    private readonly IMapper _mapper;

    public CreateReceiptVoucherHandler(IReceiptVoucherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ReceiptVoucherDto> Handle(CreateReceiptVoucherCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Accounting.ReceiptVoucher>(request.ReceiptVoucher);
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<ReceiptVoucherDto>(entity);
    }
}
