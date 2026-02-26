using AutoMapper;
using Pharmacy.Application.DTOs.IntegrationProvider;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class IntegrationProviderProfile : Profile
{
    public IntegrationProviderProfile()
    {
        CreateMap<IntegrationProvider, IntegrationProviderDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => GetStatusName(src.Status)));

        CreateMap<CreateIntegrationProviderDto, IntegrationProvider>();
        
        CreateMap<UpdateIntegrationProviderDto, IntegrationProvider>();
    }

    private static string GetStatusName(int status) => status switch
    {
        0 => "Inactive",
        1 => "Active",
        2 => "Suspended",
        _ => "Unknown"
    };
}