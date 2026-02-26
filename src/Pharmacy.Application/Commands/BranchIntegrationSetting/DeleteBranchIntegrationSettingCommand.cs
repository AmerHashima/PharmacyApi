using MediatR;

namespace Pharmacy.Application.Commands.BranchIntegrationSetting;

public record DeleteBranchIntegrationSettingCommand(Guid Id) : IRequest<bool>;