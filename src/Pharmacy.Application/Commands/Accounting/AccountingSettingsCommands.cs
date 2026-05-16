using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreateAccountingSettingsCommand(CreateAccountingSettingsDto Settings) : IRequest<AccountingSettingsDto>;
public record UpdateAccountingSettingsCommand(UpdateAccountingSettingsDto Settings) : IRequest<AccountingSettingsDto>;
public record DeleteAccountingSettingsCommand(Guid Id) : IRequest<bool>;
