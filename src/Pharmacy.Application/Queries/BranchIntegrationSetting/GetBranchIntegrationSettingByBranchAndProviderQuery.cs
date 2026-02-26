using MediatR;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;

namespace Pharmacy.Application.Queries.BranchIntegrationSetting;

public record GetBranchIntegrationSettingByBranchAndProviderQuery(Guid BranchId, Guid ProviderId) : IRequest<BranchIntegrationSettingDto?>;