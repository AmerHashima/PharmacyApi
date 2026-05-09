using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

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
        var entity = await _repository.GetByIdAsync(request.ReceiptVoucher.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"ReceiptVoucher '{request.ReceiptVoucher.Oid}' not found");

        _mapper.Map(request.ReceiptVoucher, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<ReceiptVoucherDto>(entity);
    }
}
