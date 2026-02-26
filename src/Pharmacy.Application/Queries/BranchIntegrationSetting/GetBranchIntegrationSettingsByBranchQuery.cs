using MediatR;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;

namespace Pharmacy.Application.Queries.BranchIntegrationSetting;

public record GetBranchIntegrationSettingsByBranchQuery(Guid BranchId) : IRequest<IEnumerable<BranchIntegrationSettingDto>>;