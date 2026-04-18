using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.InvoiceShape;
using Pharmacy.Application.Queries.InvoiceShape;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.InvoiceShape;

public class GetInvoiceShapeByIdHandler : IRequestHandler<GetInvoiceShapeByIdQuery, InvoiceShapeDto?>
{
    private readonly IInvoiceShapeRepository _repository;
    private readonly IMapper _mapper;

    public GetInvoiceShapeByIdHandler(IInvoiceShapeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<InvoiceShapeDto?> Handle(GetInvoiceShapeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetQueryable()
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Oid == request.Id && !x.IsDeleted, cancellationToken);

        return entity == null ? null : _mapper.Map<InvoiceShapeDto>(entity);
    }
}
