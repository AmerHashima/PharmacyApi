using AutoMapper;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class GenericNameProfile : Profile
{
    public GenericNameProfile()
    {
        CreateMap<GenericName, GenericNameDto>();
        CreateMap<CreateGenericNameDto, GenericName>();
        CreateMap<UpdateGenericNameDto, GenericName>()
            .ForMember(dest => dest.Oid,       opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt,  opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy,  opt => opt.Ignore());
    }
}
