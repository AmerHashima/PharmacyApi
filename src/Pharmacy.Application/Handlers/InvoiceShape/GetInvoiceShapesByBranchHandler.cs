using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.InvoiceShape;
using Pharmacy.Application.Queries.InvoiceShape;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.InvoiceShape;

public class GetInvoiceShapesByBranchHandler : IRequestHandler<GetInvoiceShapesByBranchQuery, IEnumerable<InvoiceShapeDto>>
{
    private readonly IInvoiceShapeRepository _repository;
    private readonly IMapper _mapper;

    public GetInvoiceShapesByBranchHandler(IInvoiceShapeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceShapeDto>> Handle(GetInvoiceShapesByBranchQuery request, CancellationToken cancellationToken)
    {
        var shapes = await _repository.GetByBranchAsync(request.BranchId, cancellationToken);
        return _mapper.Map<IEnumerable<InvoiceShapeDto>>(shapes);
    }
}
