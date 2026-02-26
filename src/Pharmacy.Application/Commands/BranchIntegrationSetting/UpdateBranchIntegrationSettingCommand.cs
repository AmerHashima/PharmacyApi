using MediatR;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;

namespace Pharmacy.Application.Commands.BranchIntegrationSetting;

public record UpdateBranchIntegrationSettingCommand(UpdateBranchIntegrationSettingDto Dto) : IRequest<BranchIntegrationSettingDto>;