using MediatR;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;

namespace Pharmacy.Application.Commands.BranchIntegrationSetting;

public record CreateBranchIntegrationSettingCommand(CreateBranchIntegrationSettingDto Dto) : IRequest<BranchIntegrationSettingDto>;