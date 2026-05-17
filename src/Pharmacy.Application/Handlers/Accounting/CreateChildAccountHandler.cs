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
/// originating customer, stakeholder, cash box, or bank account.
/// </summary>
public class CreateChildAccountHandler : IRequestHandler<CreateChildAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IStakeholderRepository _stakeholderRepository;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IMapper _mapper;

    public CreateChildAccountHandler(
        IAccountRepository accountRepository,
        ICustomerRepository customerRepository,
        IStakeholderRepository stakeholderRepository,
        ICashBoxRepository cashBoxRepository,
        IBankAccountRepository bankAccountRepository,
        IMapper mapper)
    {
        _accountRepository    = accountRepository;
        _customerRepository   = customerRepository;
        _stakeholderRepository = stakeholderRepository;
        _cashBoxRepository    = cashBoxRepository;
        _bankAccountRepository = bankAccountRepository;
        _mapper               = mapper;
    }

    public async Task<AccountDto> Handle(CreateChildAccountCommand request, CancellationToken cancellationToken)
    {
        var req = request.Request;

        int provided = (req.CustomerId.HasValue ? 1 : 0)
                     + (req.StakeholderId.HasValue ? 1 : 0)
                     + (req.CashBoxId.HasValue ? 1 : 0)
                     + (req.BankAccountId.HasValue ? 1 : 0);

        if (provided == 0)
            throw new InvalidOperationException("One of CustomerId, StakeholderId, CashBoxId, or BankAccountId must be provided.");
        if (provided > 1)
            throw new InvalidOperationException("Only one of CustomerId, StakeholderId, CashBoxId, or BankAccountId may be provided.");

        // Validate parent account exists
        var parent = await _accountRepository.GetByIdAsync(req.ParentAccountId, cancellationToken)
            ?? throw new InvalidOperationException($"Parent account '{req.ParentAccountId}' not found.");

        // Resolve display name from the linked entity
        string entityNameAr;
        string entityNameEn;

        if (req.CustomerId != null)
        {
            var customer = await _customerRepository.GetByIdAsync(req.CustomerId.Value, cancellationToken)
                ?? throw new InvalidOperationException($"Customer '{req.CustomerId}' not found.");

            entityNameAr = req.AccountNameAr ?? customer.NameEN;
            entityNameEn = req.AccountNameEn ?? customer.NameEN;
        }
        else if (req.StakeholderId != null)
        {
            var stakeholder = await _stakeholderRepository.GetByIdAsync(req.StakeholderId.Value, cancellationToken)
                ?? throw new InvalidOperationException($"Stakeholder '{req.StakeholderId}' not found.");

            entityNameAr = req.AccountNameAr ?? stakeholder.Name;
            entityNameEn = req.AccountNameEn ?? stakeholder.Name;
        }
        else if (req.CashBoxId != null)
        {
            var cashBox = await _cashBoxRepository.GetByIdAsync(req.CashBoxId.Value, cancellationToken)
                ?? throw new InvalidOperationException($"CashBox '{req.CashBoxId}' not found.");

            entityNameAr = req.AccountNameAr ?? cashBox.NameAr ?? cashBox.NameEn ?? cashBox.Code ?? "CashBox";
            entityNameEn = req.AccountNameEn ?? cashBox.NameEn ?? cashBox.NameAr ?? cashBox.Code ?? "CashBox";
        }
        else
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(req.BankAccountId!.Value, cancellationToken)
                ?? throw new InvalidOperationException($"BankAccount '{req.BankAccountId}' not found.");

            entityNameAr = req.AccountNameAr ?? bankAccount.NameAr ?? bankAccount.NameEn ?? bankAccount.Code ?? "BankAccount";
            entityNameEn = req.AccountNameEn ?? bankAccount.NameEn ?? bankAccount.NameAr ?? bankAccount.Code ?? "BankAccount";
        }

        // Auto-generate child account code when not supplied: <parentCode>.<sequence>
        string childCode = req.AccountCode ?? await GenerateChildCodeAsync(parent.Oid, parent.AccountCode, cancellationToken);

        // Build the child account
        var child = new Account
        {
            AccountCode    = childCode,
            AccountNameAr  = entityNameAr,
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

        // Link child account back to the entity
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
        else if (req.StakeholderId != null)
        {
            var stakeholder = await _stakeholderRepository.GetByIdAsync(req.StakeholderId.Value, cancellationToken);
            if (stakeholder != null)
            {
                stakeholder.ParentAccountId = parent.Oid;
                stakeholder.ChildAccountId  = child.Oid;
                await _stakeholderRepository.UpdateAsync(stakeholder, cancellationToken);
            }
        }
        else if (req.CashBoxId != null)
        {
            var cashBox = await _cashBoxRepository.GetByIdAsync(req.CashBoxId.Value, cancellationToken);
            if (cashBox != null)
            {
                cashBox.ParentAccountId = parent.Oid;
                cashBox.ChildAccountId  = child.Oid;
                await _cashBoxRepository.UpdateAsync(cashBox, cancellationToken);
            }
        }
        else
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(req.BankAccountId!.Value, cancellationToken);
            if (bankAccount != null)
            {
                bankAccount.ParentAccountId = parent.Oid;
                bankAccount.ChildAccountId  = child.Oid;
                await _bankAccountRepository.UpdateAsync(bankAccount, cancellationToken);
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

