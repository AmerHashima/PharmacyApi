using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Commands.InvoiceShape;
using Pharmacy.Application.DTOs.InvoiceShape;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.InvoiceShape;

public class UpdateInvoiceShapeHandler : IRequestHandler<UpdateInvoiceShapeCommand, InvoiceShapeDto>
{
    private readonly IInvoiceShapeRepository _repository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public UpdateInvoiceShapeHandler(IInvoiceShapeRepository repository, IBranchRepository branchRepository, IMapper mapper)
    {
        _repository = repository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<InvoiceShapeDto> Handle(UpdateInvoiceShapeCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.InvoiceShape.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Invoice shape with ID '{request.InvoiceShape.Oid}' not found");

        if (await _branchRepository.GetByIdAsync(request.InvoiceShape.BranchId, cancellationToken) == null)
            throw new KeyNotFoundException($"Branch with ID '{request.InvoiceShape.BranchId}' not found");

        if (await _repository.NameExistsAsync(request.InvoiceShape.ShapeName, request.InvoiceShape.Oid, cancellationToken))
            throw new InvalidOperationException($"Invoice shape with name '{request.InvoiceShape.ShapeName}' already exists");

        _mapper.Map(request.InvoiceShape, existing);
        await _repository.UpdateAsync(existing, cancellationToken);

        var result = await _repository.GetQueryable()
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Oid == existing.Oid, cancellationToken);

        return _mapper.Map<InvoiceShapeDto>(result);
    }
}
