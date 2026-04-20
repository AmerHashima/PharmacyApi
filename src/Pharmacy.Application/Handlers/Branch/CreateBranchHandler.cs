using AutoMapper;
using Pharmacy.Application.Commands.Branch;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using MediatR;
using InvoiceSetupEntity = Pharmacy.Domain.Entities.InvoiceSetup;

namespace Pharmacy.Application.Handlers.Branch;

/// <summary>
/// Handler for creating a new Branch
/// </summary>
public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, BranchDto>
{
    private readonly IBranchRepository _repository;
    private readonly IInvoiceSetupRepository _invoiceSetupRepository;
    private readonly IMapper _mapper;

    public CreateBranchHandler(IBranchRepository repository, IInvoiceSetupRepository invoiceSetupRepository, IMapper mapper)
    {
        _repository = repository;
        _invoiceSetupRepository = invoiceSetupRepository;
        _mapper = mapper;
    }

    public async Task<BranchDto> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        // Check if branch code already exists
        if (!await _repository.IsBranchCodeUniqueAsync(request.Branch.BranchCode, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"Branch code '{request.Branch.BranchCode}' already exists");
        }

        var branch = _mapper.Map<Domain.Entities.Branch>(request.Branch);
        var createdBranch = await _repository.AddAsync(branch, cancellationToken);

        // Auto-seed 4 InvoiceSetup rows for the new branch
        var now = DateTime.UtcNow;
        var invoiceSetups = new[]
        {
            new InvoiceSetupEntity { NameAr = "فاتورة نقاط البيع",        NameEn = "POS Invoice",             Format = IInvoiceNumberService.FormatPosInvoice,       NumberValue = 1, BranchId = createdBranch.Oid, InvoiceTypeId = IInvoiceNumberService.LookupDetailPosInvoiceId,              CreatedAt = now },
            new InvoiceSetupEntity { NameAr = "مرتجع فاتورة نقاط البيع", NameEn = "Return POS Invoice",      Format = IInvoiceNumberService.FormatReturnPosInvoice, NumberValue = 1, BranchId = createdBranch.Oid, InvoiceTypeId = IInvoiceNumberService.LookupDetailReturnPosInvoiceId,        CreatedAt = now },
            new InvoiceSetupEntity { NameAr = "فاتورة المورد",            NameEn = "Supplier Invoice",        Format = IInvoiceNumberService.FormatSupplierInvoice,  NumberValue = 1, BranchId = createdBranch.Oid, InvoiceTypeId = IInvoiceNumberService.LookupDetailSupplierInvoiceId,         CreatedAt = now },
            new InvoiceSetupEntity { NameAr = "مرتجع فاتورة المورد",     NameEn = "Return Supplier Invoice", Format = IInvoiceNumberService.FormatReturnSupplier,   NumberValue = 1, BranchId = createdBranch.Oid, InvoiceTypeId = IInvoiceNumberService.LookupDetailReturnSupplierInvoiceId,  CreatedAt = now },
        };

        foreach (var setup in invoiceSetups)
            await _invoiceSetupRepository.AddAsync(setup, cancellationToken);

        return _mapper.Map<BranchDto>(createdBranch);
    }
}
