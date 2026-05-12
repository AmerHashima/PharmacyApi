using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
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
        var master = _mapper.Map<ReceiptVoucher>(request.ReceiptVoucher);
        master.CreatedAt   = DateTime.UtcNow;
        master.TotalAmount = request.ReceiptVoucher.Details.Sum(d => d.Amount);

        var details = _mapper.Map<List<ReceiptVoucherDetail>>(request.ReceiptVoucher.Details);
        foreach (var detail in details)
        {
            detail.ReceiptVoucherId = master.Oid;
            detail.CreatedAt = DateTime.UtcNow;
        }

        await _repository.InsertMasterDetailAsync(master, details, cancellationToken);

        var created = await _repository.GetWithDetailsAsync(master.Oid, cancellationToken);
        return _mapper.Map<ReceiptVoucherDto>(created);
    }
}
