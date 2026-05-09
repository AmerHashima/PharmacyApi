using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreateBankAccountHandler : IRequestHandler<CreateBankAccountCommand, BankAccountDto>
{
    private readonly IBankAccountRepository _repository;
    private readonly IMapper _mapper;

    public CreateBankAccountHandler(IBankAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BankAccountDto> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Accounting.BankAccount>(request.BankAccount);
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<BankAccountDto>(entity);
    }
}
