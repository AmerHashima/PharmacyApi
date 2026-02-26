using AutoMapper;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class BranchIntegrationSettingProfile : Profile
{
    public BranchIntegrationSettingProfile()
    {
        CreateMap<BranchIntegrationSetting, BranchIntegrationSettingDto>()
            .ForMember(dest => dest.IntegrationProviderName, opt => opt.MapFrom(src => src.IntegrationProvider.Name))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => GetStatusName(src.Status)));

        CreateMap<CreateBranchIntegrationSettingDto, BranchIntegrationSetting>();
        
        CreateMap<UpdateBranchIntegrationSettingDto, BranchIntegrationSetting>();
    }

    private static string GetStatusName(int status) => status switch
    {
        0 => "Inactive",
        1 => "Active",
        2 => "Testing",
        _ => "Unknown"
    };
}