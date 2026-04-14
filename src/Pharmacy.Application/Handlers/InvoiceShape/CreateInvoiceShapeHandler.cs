using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Commands.InvoiceShape;
using Pharmacy.Application.DTOs.InvoiceShape;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.InvoiceShape;

public class CreateInvoiceShapeHandler : IRequestHandler<CreateInvoiceShapeCommand, InvoiceShapeDto>
{
    private readonly IInvoiceShapeRepository _repo;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public CreateInvoiceShapeHandler(IInvoiceShapeRepository repo, IBranchRepository branchRepository, IMapper mapper)
    {
        _repo = repo;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<InvoiceShapeDto> Handle(CreateInvoiceShapeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.InvoiceShape;

        var branch = await _branchRepository.GetByIdAsync(dto.BranchId, cancellationToken)
            ?? throw new KeyNotFoundException($"Branch with ID '{dto.BranchId}' not found");

        if (await _repo.NameExistsAsync(dto.ShapeName, cancellationToken: cancellationToken))
            throw new InvalidOperationException($"Invoice shape with name '{dto.ShapeName}' already exists");

        var entity = _mapper.Map<Domain.Entities.InvoiceShape>(dto);
        var created = await _repo.AddAsync(entity, cancellationToken);

        var result = await _repo.GetQueryable()
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Oid == created.Oid, cancellationToken);

        return _mapper.Map<InvoiceShapeDto>(result);
    }
}
