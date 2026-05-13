using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Creates a child <see cref="Account"/> under a given parent account and links it back to the
/// originating customer or stakeholder (supplier).
/// </summary>
public class CreateChildAccountHandler : IRequestHandler<CreateChildAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IStakeholderRepository _stakeholderRepository;
    private readonly IMapper _mapper;

    public CreateChildAccountHandler(
        IAccountRepository accountRepository,
        ICustomerRepository customerRepository,
        IStakeholderRepository stakeholderRepository,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
        _stakeholderRepository = stakeholderRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(CreateChildAccountCommand request, CancellationToken cancellationToken)
    {
        var req = request.Request;

        if (req.CustomerId == null && req.StakeholderId == null)
            throw new InvalidOperationException("Either CustomerId or StakeholderId must be provided.");

        if (req.CustomerId != null && req.StakeholderId != null)
            throw new InvalidOperationException("Only one of CustomerId or StakeholderId may be provided.");

        // Validate parent account exists
        var parent = await _accountRepository.GetByIdAsync(req.ParentAccountId, cancellationToken)
            ?? throw new InvalidOperationException($"Parent account '{req.ParentAccountId}' not found.");

        // Resolve display name and entity type
        string entityName;
        string entityNameEn;

        if (req.CustomerId != null)
        {
            var customer = await _customerRepository.GetByIdAsync(req.CustomerId.Value, cancellationToken)
                ?? throw new InvalidOperationException($"Customer '{req.CustomerId}' not found.");

            entityName   = req.AccountNameAr ?? customer.NameEN;
            entityNameEn = req.AccountNameEn ?? customer.NameEN;
        }
        else
        {
            var stakeholder = await _stakeholderRepository.GetByIdAsync(req.StakeholderId!.Value, cancellationToken)
                ?? throw new InvalidOperationException($"Stakeholder '{req.StakeholderId}' not found.");

            entityName   = req.AccountNameAr ?? stakeholder.Name;
            entityNameEn = req.AccountNameEn ?? stakeholder.Name;
        }

        // Auto-generate child account code when not supplied: <parentCode>.<sequence>
        string childCode = req.AccountCode ?? await GenerateChildCodeAsync(parent.Oid, parent.AccountCode, cancellationToken);

        // Build the child account
        var child = new Account
        {
            AccountCode    = childCode,
            AccountNameAr  = entityName,
            AccountNameEn  = entityNameEn,
            ParentId       = parent.Oid,
            AccountLevel   = parent.AccountLevel + 1,
            AccountTypeId  = parent.AccountTypeId,
            NatureId       = parent.NatureId,
            FinalAccountId = parent.FinalAccountId,
            IsLeaf         = true,
            IsActive       = true,
            CreatedAt      = DateTime.UtcNow,
        };

        await _accountRepository.AddAsync(child, cancellationToken);

        // Mark parent as non-leaf now that it has a child
        if (parent.IsLeaf)
        {
            parent.IsLeaf = false;
            await _accountRepository.UpdateAsync(parent, cancellationToken);
        }

        // Link child account back to the customer / stakeholder
        if (req.CustomerId != null)
        {
            var customer = await _customerRepository.GetByIdAsync(req.CustomerId.Value, cancellationToken);
            if (customer != null)
            {
                customer.ParentAccountId = parent.Oid;
                customer.ChildAccountId  = child.Oid;
                await _customerRepository.UpdateAsync(customer, cancellationToken);
            }
        }
        else
        {
            var stakeholder = await _stakeholderRepository.GetByIdAsync(req.StakeholderId!.Value, cancellationToken);
            if (stakeholder != null)
            {
                stakeholder.ParentAccountId = parent.Oid;
                stakeholder.ChildAccountId  = child.Oid;
                await _stakeholderRepository.UpdateAsync(stakeholder, cancellationToken);
            }
        }

        return _mapper.Map<AccountDto>(child);
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private async Task<string> GenerateChildCodeAsync(Guid parentId, string parentCode, CancellationToken cancellationToken)
    {
        var siblings = await _accountRepository.GetByParentAsync(parentId, cancellationToken);
        int sequence = siblings.Count() + 1;
        return $"{parentCode}.{sequence:D3}";
    }
}
