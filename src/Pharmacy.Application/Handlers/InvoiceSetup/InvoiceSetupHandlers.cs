using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Commands.InvoiceSetup;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.InvoiceSetup;
using Pharmacy.Application.Queries.InvoiceSetup;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.InvoiceSetup;

// ── GetById ──────────────────────────────────────────────────────────────────
public class GetInvoiceSetupByIdHandler : IRequestHandler<GetInvoiceSetupByIdQuery, InvoiceSetupDto?>
{
    private readonly IInvoiceSetupRepository _repo;
    private readonly IMapper _mapper;

    public GetInvoiceSetupByIdHandler(IInvoiceSetupRepository repo, IMapper mapper)
    {
        _repo   = repo;
        _mapper = mapper;
    }

    public async Task<InvoiceSetupDto?> Handle(GetInvoiceSetupByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetQueryable()
            .Include(x => x.Branch)
            .Where(x => x.Oid == request.Id && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<InvoiceSetupDto>(entity);
    }
}

// ── GetByBranch ───────────────────────────────────────────────────────────────
public class GetInvoiceSetupsByBranchHandler : IRequestHandler<GetInvoiceSetupsByBranchQuery, IEnumerable<InvoiceSetupDto>>
{
    private readonly IInvoiceSetupRepository _repo;
    private readonly IMapper _mapper;

    public GetInvoiceSetupsByBranchHandler(IInvoiceSetupRepository repo, IMapper mapper)
    {
        _repo   = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceSetupDto>> Handle(GetInvoiceSetupsByBranchQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repo.GetByBranchIdAsync(request.BranchId, cancellationToken);
        return _mapper.Map<IEnumerable<InvoiceSetupDto>>(entities);
    }
}

// ── GetGlobal ─────────────────────────────────────────────────────────────────
public class GetGlobalInvoiceSetupsHandler : IRequestHandler<GetGlobalInvoiceSetupsQuery, IEnumerable<InvoiceSetupDto>>
{
    private readonly IInvoiceSetupRepository _repo;
    private readonly IMapper _mapper;

    public GetGlobalInvoiceSetupsHandler(IInvoiceSetupRepository repo, IMapper mapper)
    {
        _repo   = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceSetupDto>> Handle(GetGlobalInvoiceSetupsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repo.GetGlobalTemplatesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<InvoiceSetupDto>>(entities);
    }
}

// ── GetData (paged) ───────────────────────────────────────────────────────────
public class GetInvoiceSetupDataHandler : IRequestHandler<GetInvoiceSetupDataQuery, PagedResult<InvoiceSetupDto>>
{
    private readonly IInvoiceSetupRepository _repo;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetInvoiceSetupDataHandler(IInvoiceSetupRepository repo, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repo         = repo;
        _queryBuilder = queryBuilder;
        _mapper       = mapper;
    }

    public async Task<PagedResult<InvoiceSetupDto>> Handle(GetInvoiceSetupDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetQueryable()
            .Include(x => x.Branch)
            .Where(x => !x.IsDeleted);

        query = _queryBuilder.ApplyFilters(query, request.Request.Request.Filters);
        query = _queryBuilder.ApplySorting(query, request.Request.Request.Sort);

        var paged = await _queryBuilder.ApplyPaginationAsync(query, request.Request.Request.Pagination);
        var dtos  = _mapper.Map<List<InvoiceSetupDto>>(paged.Data);

        return PagedResult<InvoiceSetupDto>.Create(dtos, paged.TotalRecords, paged.PageNumber, paged.PageSize);
    }
}

// ── Create ────────────────────────────────────────────────────────────────────
public class CreateInvoiceSetupHandler : IRequestHandler<CreateInvoiceSetupCommand, InvoiceSetupDto>
{
    private readonly IInvoiceSetupRepository _repo;
    private readonly IMapper _mapper;

    public CreateInvoiceSetupHandler(IInvoiceSetupRepository repo, IMapper mapper)
    {
        _repo   = repo;
        _mapper = mapper;
    }

    public async Task<InvoiceSetupDto> Handle(CreateInvoiceSetupCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Pharmacy.Domain.Entities.InvoiceSetup>(request.Dto);
        await _repo.AddAsync(entity, cancellationToken);
        return _mapper.Map<InvoiceSetupDto>(entity);
    }
}

// ── Update ────────────────────────────────────────────────────────────────────
public class UpdateInvoiceSetupHandler : IRequestHandler<UpdateInvoiceSetupCommand, InvoiceSetupDto>
{
    private readonly IInvoiceSetupRepository _repo;
    private readonly IMapper _mapper;

    public UpdateInvoiceSetupHandler(IInvoiceSetupRepository repo, IMapper mapper)
    {
        _repo   = repo;
        _mapper = mapper;
    }

    public async Task<InvoiceSetupDto> Handle(UpdateInvoiceSetupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Dto.Oid, cancellationToken)
            ?? throw new InvalidOperationException($"InvoiceSetup '{request.Dto.Oid}' not found.");

        _mapper.Map(request.Dto, entity);
        await _repo.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<InvoiceSetupDto>(entity);
    }
}

// ── Delete ────────────────────────────────────────────────────────────────────
public class DeleteInvoiceSetupHandler : IRequestHandler<DeleteInvoiceSetupCommand, bool>
{
    private readonly IInvoiceSetupRepository _repo;

    public DeleteInvoiceSetupHandler(IInvoiceSetupRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteInvoiceSetupCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
